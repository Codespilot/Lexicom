using Lexicom.UnitTesting.DependencyInjection.Exceptions;
using Lexicom.UnitTesting.DependencyInjection.Utility;
using NSubstitute;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Lexicom.UnitTesting.DependencyInjection.Mocking;

public class MockManager : IDisposable, IReadOnlyDictionary<Type, MockContainer>
{
    public MockManager(TestAssistantConfiguration configuration)
    {
        AssistantConfiguration = configuration;
        MockTypeToContainer = [];
    }

    private TestAssistantConfiguration AssistantConfiguration { get; }
    private Dictionary<Type, MockContainer> MockTypeToContainer { get; }

    public IEnumerable<Type> Keys => MockTypeToContainer.Keys;
    public IEnumerable<MockContainer> Values => MockTypeToContainer.Values;
    public int Count => MockTypeToContainer.Count;

    public MockContainer this[Type key] => MockTypeToContainer[key];

    public void Dispose()
    {
        foreach (MockContainer container in MockTypeToContainer.Values)
        {
            container?.Dispose();
        }
    }

    public UnitTestAssistantMockFluentBuilder<TService> Mock<TService>() where TService : class
    {
        return Mock<TService>(AssistantConfiguration.DefaultMockLifetime);
    }
    public UnitTestAssistantMockFluentBuilder<TService> Mock<TService>(MockLifetime lifetime) where TService : class
    {
        var container = new MockContainer<TService>(this, lifetime);

        MockTypeToContainer.Add(container.ServiceType, container);

        return new UnitTestAssistantMockFluentBuilder<TService>(container);
    }
    internal UnitTestAssistantMockFluentBuilder Mock(Type serviceType, MockLifetime lifetime)
    {
        var container = new MockContainer(this, serviceType, lifetime);

        MockTypeToContainer.Add(container.ServiceType, container);

        return new UnitTestAssistantMockFluentBuilder(container);
    }

    /// <exception cref="PullValueTypeException"></exception>
    /// <exception cref="PullNotMockedException"></exception>
    public object Pull(Type type)
    {
        if (AssistantConfiguration.IsAutomaticallyMocking)
        {
            if (!MockTypeToContainer.ContainsKey(type))
            {
                if (TypeUtilities.IsValueType(type))
                {
                    throw new PullValueTypeException(type);
                }

                if (!MockHooksManager.TryToMockFromHooks(this, type, AssistantConfiguration))
                {
                    Mock(type, AssistantConfiguration.DefaultMockLifetime);
                }
            }
        }

        if (!MockTypeToContainer.TryGetValue(type, out MockContainer? container))
        {
            throw new PullNotMockedException(type);
        }

        return container.Pull();
    }

    public object CreateSubstitute(Type substituteType)
    {
        ConstructorInfo[] constructors = substituteType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        //we go ahead and create substitutes for all the required constructor parameters for this substitute this
        //only applies when this substitute is a concrete type and not an interface which should be rare hopefully
        object[] constructorArguments = [];
        if (constructors.Length > 0)
        {
            ParameterInfo[] parameters = constructors
                .First()
                .GetParameters();

            constructorArguments = new object[parameters.Length];
            for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                ParameterInfo parameter = parameters[parameterIndex];
                Type parameterType = parameter.ParameterType;

                object instance = Pull(parameterType);

                constructorArguments[parameterIndex] = instance;
            }
        }

        return Substitute.For(typesToProxy: [substituteType], constructorArguments);
    }

    public bool ContainsKey(Type key) => MockTypeToContainer.ContainsKey(key);
    public bool TryGetValue(Type key, [MaybeNullWhen(false)] out MockContainer value) => MockTypeToContainer.TryGetValue(key, out value);
    public IEnumerator<KeyValuePair<Type, MockContainer>> GetEnumerator() => MockTypeToContainer.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}