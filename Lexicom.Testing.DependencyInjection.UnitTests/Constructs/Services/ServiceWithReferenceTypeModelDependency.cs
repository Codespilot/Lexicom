using Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Models;

namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public class ServiceWithReferenceTypeModelDependency
{
    public ServiceWithReferenceTypeModelDependency(ReferenceTypeModel referenceTypeModel)
    {
        ReferenceTypeModel = referenceTypeModel;
    }

    public ReferenceTypeModel ReferenceTypeModel { get; }
}
