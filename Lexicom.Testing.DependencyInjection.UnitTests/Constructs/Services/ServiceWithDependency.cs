namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithDependency
{
    public readonly IServiceDependencyIntReturnMethod _serviceDependencyIntReturnMethod;

    public ServiceWithDependency(IServiceDependencyIntReturnMethod serviceDependencyIntReturnMethod)
    {
        _serviceDependencyIntReturnMethod = serviceDependencyIntReturnMethod;
    }
}
