using Lexicom.Testing.DependencyInjection.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections;
using System.Reflection;

namespace Lexicom.Testing.DependencyInjection;

public class IntegrationTestAssistant : TestAssistant, IServiceCollection
{
    private readonly IServiceCollection _services;

    public IntegrationTestAssistant() : this(new TestAssistantConfiguration())
    {
    }
    public IntegrationTestAssistant(TestAssistantConfiguration configuration) : base(configuration)
    {
        _services = new ServiceCollection();

        Configuration = new ConfigurationManager();
    }

    public int Count => _services.Count;
    public bool IsReadOnly => _services.IsReadOnly;
    public ConfigurationManager Configuration { get; }
    private IServiceProvider? Provider { get; set; }

    public ServiceDescriptor this[int index]
    {
        get => _services[index];
        set => _services[index] = value;
    }

    protected override Type GetMakeType(Type serviceType)
    {
        ServiceDescriptor? serviceDescriptor = _services.FirstOrDefault(sd => sd.ServiceType == serviceType);

        if (serviceDescriptor is null && serviceType.IsConstructedGenericType)
        {
            var openGeneric = serviceType.GetGenericTypeDefinition();

            serviceDescriptor = _services.FirstOrDefault(sd => sd.ServiceType == openGeneric);
        }

        Type makeType;
        if (serviceType.IsInterface)
        {
            if (serviceDescriptor is null)
            {
                throw new MakeTypeIsNoRegisteredException(serviceType);
            }

            if (serviceDescriptor.ImplementationType is null)
            {
                throw new NotImplementedException("Need to implement handler for factory?");
            }

            makeType = serviceDescriptor.ImplementationType;
        }
        else
        {
            makeType = serviceType;

            if (serviceDescriptor is null)
            {
                Provider = null;
                _services.Add(new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Singleton));
            }
        }

        return makeType;
    }

    protected override void ResolveParameter(int parameterIndex, object[] resolvedParameters, Type parameterType)
    {
        ServiceDescriptor? existingServiceDescriptor = _services.FirstOrDefault(sd => sd.ServiceType == parameterType);
        if (existingServiceDescriptor is null)
        {
            Provider = null;
            _services.Add(new ServiceDescriptor(parameterType, sp =>
            {
                return PullAndEnhanceInstance(parameterType);
            }, ServiceLifetime.Singleton));
        }
    }

    protected override object MakeInstance(Type type, Type makeType, ConstructorInfo constructor, object[] resolvedParameters)
    {
        if (Provider is null)
        {
            _services.TryAddSingleton<IConfiguration>(Configuration);

            Provider = _services.BuildServiceProvider();
        }

        //for integration tests the resolved parameters are only used for manually provided parameters
        object[] manualParameters = resolvedParameters
            .Where(rp => rp is not null)
            .ToArray();

        if (manualParameters.Length > 0)
        {
            return ActivatorUtilities.CreateInstance(Provider, makeType, manualParameters);
        }

        return Provider.GetRequiredService(type);
    }

    public int IndexOf(ServiceDescriptor item) => _services.IndexOf(item);

    public bool Contains(ServiceDescriptor item) => _services.Contains(item);

    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="ArgumentException"/>
    public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _services.CopyTo(array, arrayIndex);

    /// <exception cref="NotSupportedException"/>
    public void Clear() => _services.Clear();

    /// <exception cref="NotSupportedException"/>
    public void Add(ServiceDescriptor item) => _services.Add(item);

    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="NotSupportedException"/>
    public void Insert(int index, ServiceDescriptor item) => _services.Insert(index, item);

    /// <exception cref="NotSupportedException"/>
    public bool Remove(ServiceDescriptor item) => _services.Remove(item);

    /// <exception cref="ArgumentOutOfRangeException"/>
    /// <exception cref="NotSupportedException"/>
    public void RemoveAt(int index) => _services.RemoveAt(index);

    public IEnumerator<ServiceDescriptor> GetEnumerator() => _services.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
