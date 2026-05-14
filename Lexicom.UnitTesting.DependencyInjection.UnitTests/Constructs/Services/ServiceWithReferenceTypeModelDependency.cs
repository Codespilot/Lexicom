using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Models;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithReferenceTypeModelDependency
{
    public ServiceWithReferenceTypeModelDependency(ReferenceTypeModel referenceTypeModel)
    {
        ReferenceTypeModel = referenceTypeModel;
    }

    public ReferenceTypeModel ReferenceTypeModel { get; }
}
