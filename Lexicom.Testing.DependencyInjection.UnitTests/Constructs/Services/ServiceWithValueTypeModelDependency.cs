using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Models;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithValueTypeModelDependency
{
    public ServiceWithValueTypeModelDependency(ValueTypeModel valueTypeModel)
    {
        ValueTypeModel = valueTypeModel;
    }

    public ValueTypeModel ValueTypeModel { get; }
}
