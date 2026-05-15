namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithNullableValueTypeDependency
{
    public ServiceWithNullableValueTypeDependency(int? nullableIntType)
    {
        NullableIntType = nullableIntType;
    }

    public int? NullableIntType { get; }
}
