using Lexicom.UnitTesting.DependencyInjection.Exceptions;
using Lexicom.UnitTesting.DependencyInjection.Extensions;
using Lexicom.UnitTesting.DependencyInjection.Mocking;
using Lexicom.UnitTesting.DependencyInjection.Utility;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Lexicom.UnitTesting.DependencyInjection;

public class UnitTestAssistant : IDisposable
{
    public UnitTestAssistant() : this(new UnitTestAssistantConfiguration())
    {
    }
    public UnitTestAssistant(UnitTestAssistantConfiguration configuration)
    {
        Configuration = configuration;

        MockManager = new MockManager(Configuration);
    }

    private UnitTestAssistantConfiguration Configuration { get; }
    internal MockManager MockManager { get; }

    public void Dispose()
    {
        MockManager.Dispose();
    }

    public UnitTestAssistantMockFluentBuilder<TService> Mock<TService>() where TService : class => MockManager.Mock<TService>();
    public UnitTestAssistantMockFluentBuilder<TService> Mock<TService>(MockLifetime lifetime) where TService : class => MockManager.Mock<TService>(lifetime);

    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="MakeZeroConstructorsException"></exception>
    /// <exception cref="MakeTooManyConstructorsException"></exception>
    /// <exception cref="PullValueTypeException"></exception>
    /// <exception cref="PullNotMockedException"></exception>
    /// <exception cref="MakeUnusedManualParametersException"></exception>
    public T Make<T>(params object[] manualParameters) where T : class
    {
        return (T)Make(typeof(T), manualParameters);
    }
    private object Make(Type type, object[] manualParameters)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(manualParameters);

        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        if (constructors.Length <= 0)
        {
            throw new MakeZeroConstructorsException(type);
        }

        if (constructors.Length > 1)
        {
            throw new MakeTooManyConstructorsException(type, constructors.Length);
        }

        bool[] isManualParameterUsed = new bool[manualParameters.Length];

        ConstructorInfo constructor = constructors.Single();
        ParameterInfo[] parameters = constructor.GetParameters();

        object[] resolvedParameters = new object[parameters.Length];
        for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
        {
            bool isResolved = false;
            ParameterInfo parameter = parameters[parameterIndex];
            Type parameterType = parameter.ParameterType;

            for (int manualParameterIndex = 0; manualParameterIndex < manualParameters.Length; manualParameterIndex++)
            {
                if (!isManualParameterUsed[manualParameterIndex])
                {
                    object candidate = manualParameters[manualParameterIndex];

                    if (TypeUtilities.IsAssignableTo(candidate, parameterType))
                    {
                        isManualParameterUsed[manualParameterIndex] = true;

                        resolvedParameters[parameterIndex] = candidate;

                        isResolved = true;

                        break;
                    }
                }
            }

            if (!isResolved)
            {
                object instance = MockManager.Pull(parameterType);

                if (Configuration.IsEnhancedOptionsMocking && instance.IsSubstitute() && parameterType.IsGenericType)
                {
                    Type substituteGenericType = parameterType.GetGenericTypeDefinition();

                    object? optionValue;
                    if (TryPullOptionsValueObject(typeof(IOptions<>), substituteGenericType, parameterType, out optionValue))
                    {
                        IOptions<object> optionsInstance = (IOptions<object>)instance;

                        optionsInstance.Value.Returns(optionValue);
                    }
                    else if (TryPullOptionsValueObject(typeof(IOptionsMonitor<>), substituteGenericType, parameterType, out optionValue))
                    {
                        IOptionsMonitor<object> optionsMonitorInstance = (IOptionsMonitor<object>)instance;

                        optionsMonitorInstance.CurrentValue.Returns(optionValue);
                    }
                    else if (TryPullOptionsValueObject(typeof(IOptionsSnapshot<>), substituteGenericType, parameterType, out optionValue))
                    {
                        IOptionsSnapshot<object> optionsSnapshotInstance = (IOptionsSnapshot<object>)instance;

                        optionsSnapshotInstance.Value.Returns(optionValue);
                        optionsSnapshotInstance.Get(Arg.Any<string>()).Returns(optionValue);
                    }
                }

                resolvedParameters[parameterIndex] = instance;
            }
        }

        var unusedManualParameterTypeNames = new HashSet<string>();
        for (int manualParameterIndex = 0; manualParameterIndex < manualParameters.Length; manualParameterIndex++)
        {
            if (!isManualParameterUsed[manualParameterIndex])
            {
                string typeName = manualParameters[manualParameterIndex]?.GetType().FullName ?? "null";

                unusedManualParameterTypeNames.Add($"[{manualParameterIndex}:{typeName}]");
            }
        }

        if (unusedManualParameterTypeNames.Count > 0)
        {
            throw new MakeUnusedManualParametersException(type, unusedManualParameterTypeNames);
        }

        return constructor.Invoke(resolvedParameters);
    }

    private bool TryPullOptionsValueObject(Type genericOptionsType, Type substituteGenericType, Type substituteType, [NotNullWhen(true)] out object? optionValue)
    {
        if (substituteGenericType == genericOptionsType)
        {
            Type? optionsValueType = substituteType
                .GetGenericArguments()
                .FirstOrDefault();

            if (optionsValueType is null)
            {
                throw new UnreachableException($"The substitute type '{substituteType.Name}' had no generic argument.");
            }

            optionValue = MockManager.Pull(optionsValueType);

            //set all string properties of the option value object to string.Empty instead of null
            IEnumerable<PropertyInfo> writableStringProperties = optionsValueType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (PropertyInfo stringProperty in writableStringProperties)
            {
                object? value = stringProperty.GetValue(optionValue);
                if (value is null)
                {
                    stringProperty?.SetValue(optionValue, string.Empty);
                }
            }

            return true;
        }

        optionValue = null;

        return false;
    }
}
