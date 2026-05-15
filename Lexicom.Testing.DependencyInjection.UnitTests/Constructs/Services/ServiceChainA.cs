namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceChainA
{
    public readonly ServiceChainB _serviceChainB;

    public ServiceChainA(ServiceChainB serviceChainB)
    {
        _serviceChainB = serviceChainB;
    }

    public string GetAText()
    {
        return $"a{_serviceChainB.GetBText()}";
    }
}
