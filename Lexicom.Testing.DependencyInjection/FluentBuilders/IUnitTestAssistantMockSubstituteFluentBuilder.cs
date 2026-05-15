namespace Lexicom.Testing.DependencyInjection;

public interface IUnitTestAssistantMockSubstituteFluentBuilder<T> where T : class
{
    /// <exception cref="ArgumentNullException"></exception>
    void So(Action<T> substitutions);
}
