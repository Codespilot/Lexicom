using Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Models;

namespace Lexicom.Testing.DependencyInjection.UnitTests.Constructs.Services;

public interface IServiceDependencyReferenceTypeReturnMethod
{
    ReferenceTypeModel GetReferenceTypeModelMethod();
    Task<ReferenceTypeModel> GetReferenceTypeModelMethodAsync();
}
