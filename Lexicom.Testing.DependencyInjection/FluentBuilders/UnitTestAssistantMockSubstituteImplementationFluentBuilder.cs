using Lexicom.Testing.DependencyInjection.Mocking;

namespace Lexicom.Testing.DependencyInjection;

public class UnitTestAssistantMockSubstituteImplementationFluentBuilder<TService, TImplementation> : IUnitTestAssistantMockSubstituteFluentBuilder<TImplementation> where TService : class where TImplementation : class, TService
{
    /// <exception cref="ArgumentNullException"></exception>
    public UnitTestAssistantMockSubstituteImplementationFluentBuilder(MockContainer<TService> container)
    {
        ArgumentNullException.ThrowIfNull(container);

        Container = container;
    }

    protected MockContainer<TService> Container { get; }

    /// <exception cref="ArgumentNullException"></exception>
    public virtual void So(Action<TImplementation> substitutions)
    {
        ArgumentNullException.ThrowIfNull(substitutions);

        Container.SetConfigureDelegate(substitutions);
    }
}
