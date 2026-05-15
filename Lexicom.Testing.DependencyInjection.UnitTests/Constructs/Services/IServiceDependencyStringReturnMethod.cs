namespace Lexicom.UnitTesting.DependencyInjection.UnitTests.Constructs.Services;

public interface IServiceDependencyStringReturnMethod
{
    string GetStringMethod();
    Task<string> GetStringAsync();
}
