using Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Models;

namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithValueTypeModelDependency
{
    public ServiceWithValueTypeModelDependency(ValueTypeModel valueTypeModel)
    {
        ValueTypeModel = valueTypeModel;
    }

    public ValueTypeModel ValueTypeModel { get; }
}
