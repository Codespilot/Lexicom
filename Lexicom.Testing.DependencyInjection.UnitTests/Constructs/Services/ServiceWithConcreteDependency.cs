namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithConcreteDependency
{
    public readonly AnotherServiceWithConcreteDependency _anotherServiceWithConcreteDependency;

    public ServiceWithConcreteDependency(AnotherServiceWithConcreteDependency anotherServiceWithConcreteDependency)
    {
        _anotherServiceWithConcreteDependency = anotherServiceWithConcreteDependency;
    }
}
