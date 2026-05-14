using Lexicom.UnitTesting.DependencyInjection.Mocking;

namespace Lexicom.UnitTesting.DependencyInjection;

public class UnitTestAssistantMockFluentBuilder
{
    /// <exception cref="ArgumentNullException"></exception>
    public UnitTestAssistantMockFluentBuilder(MockContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        Container = container;
    }

    protected MockContainer Container { get; }

    /// <exception cref="ArgumentNullException"></exception>
    public void With(Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(implementationType);

        Container.Instantiater.Set(implementationType);
    }
}
public class UnitTestAssistantMockFluentBuilder<TService> : UnitTestAssistantMockFluentBuilder, IUnitTestAssistantMockSubstituteFluentBuilder<TService> where TService : class
{
    /// <exception cref="ArgumentNullException"></exception>
    public UnitTestAssistantMockFluentBuilder(MockContainer<TService> container) : base(container)
    {
        ArgumentNullException.ThrowIfNull(container);

        GenericContainer = container;
    }

    protected MockContainer<TService> GenericContainer { get; }

    /// <exception cref="ArgumentNullException"></exception>
    public void So(Action<TService> substitutions)
    {
        ArgumentNullException.ThrowIfNull(substitutions);

        GenericContainer.SetConfigureDelegate(substitutions);
    }

    public IUnitTestAssistantMockSubstituteFluentBuilder<TImplementation> With<TImplementation>() where TImplementation : class, TService
    {
        Container.Instantiater.Set(typeof(TImplementation));

        return new UnitTestAssistantMockSubstituteImplementationFluentBuilder<TService, TImplementation>(GenericContainer);
    }

    public void With<TImplementation>(TImplementation instance) where TImplementation : class, TService
    {
        Container.Instantiater.Set(instance);
    }

    public void With<TImplementation>(Func<TImplementation> factory) where TImplementation : class, TService
    {
        Container.Instantiater.Set(factory);
    }
}
