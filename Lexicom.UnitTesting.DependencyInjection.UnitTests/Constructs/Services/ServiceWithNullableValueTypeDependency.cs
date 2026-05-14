namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithNullableValueTypeDependency
{
    public ServiceWithNullableValueTypeDependency(int? nullableIntType)
    {
        NullableIntType = nullableIntType;
    }

    public int? NullableIntType { get; }
}
