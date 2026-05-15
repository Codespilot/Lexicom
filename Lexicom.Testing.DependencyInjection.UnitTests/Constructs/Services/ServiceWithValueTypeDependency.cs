namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithValueTypeDependency
{
    public ServiceWithValueTypeDependency(int intValueType)
    {
        IntValueType = intValueType;
    }

    public int IntValueType { get; }
}
