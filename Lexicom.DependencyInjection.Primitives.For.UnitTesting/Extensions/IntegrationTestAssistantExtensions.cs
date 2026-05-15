using Lexicom.Testing.DependencyInjection;

namespace Lexicom.DependencyInjection.Primitives.For.UnitTesting.Extensions;

public static class IntegrationTestAssistantExtensions
{
    /// <exception cref="ArgumentNullException"/>
    public static void AddLexicomPrimitivesForTesting(this IntegrationTestAssistant integrationTestAssistant, Action<ITestDependencyInjectionPrimitivesServiceBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(integrationTestAssistant);

        configure?.Invoke(new TestDependencyInjectionPrimitivesServiceBuilder(integrationTestAssistant));
    }
}
