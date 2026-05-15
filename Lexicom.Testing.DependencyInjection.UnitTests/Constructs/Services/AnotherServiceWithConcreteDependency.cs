namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public class AnotherServiceWithConcreteDependency
{
    public readonly ServiceWithNoDependencies _serviceWithNoDependencies;

    public AnotherServiceWithConcreteDependency(ServiceWithNoDependencies serviceWithNoDependencies)
    {
        _serviceWithNoDependencies = serviceWithNoDependencies;
    }
}
