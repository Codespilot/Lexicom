namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceChain
{
    public readonly ServiceChainA _serviceChainA;
    public readonly ServiceChainB _serviceChainB;
    public readonly ServiceChainB _serviceChainC;

    public ServiceChain(
        ServiceChainA serviceChainA,
        ServiceChainB serviceChainB,
        ServiceChainB serviceChainC)
    {
        _serviceChainA = serviceChainA;
        _serviceChainB = serviceChainB;
        _serviceChainC = serviceChainC;
    }
}
