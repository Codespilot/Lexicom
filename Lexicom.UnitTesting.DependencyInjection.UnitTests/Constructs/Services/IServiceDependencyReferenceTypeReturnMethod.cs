using Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Models;

namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public interface IServiceDependencyReferenceTypeReturnMethod
{
    ReferenceTypeModel GetReferenceTypeModelMethod();
    Task<ReferenceTypeModel> GetReferenceTypeModelMethodAsync();
}
