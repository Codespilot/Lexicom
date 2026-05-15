using Lexicom.Mvvm.Extensions;
using Lexicom.Supports.Testing;

namespace Lexicom.Mvvm.For.Testing.Extensions;

public static class TestingServiceBuilderExtensions
{
    /// <exception cref="ArgumentNullException"/>
    public static ITestingServiceBuilder AddMvvm(this ITestingServiceBuilder builder, Action<IMvvmServiceBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddLexicomMvvm(configure);

        return builder;
    }
}
