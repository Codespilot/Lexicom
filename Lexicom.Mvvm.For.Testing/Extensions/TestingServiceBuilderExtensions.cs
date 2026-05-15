using Lexicom.Mvvm.Extensions;
using Lexicom.Supports.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Lexicom.Mvvm.For.Testing.Extensions;

public static class TestingServiceBuilderExtensions
{
    /// <exception cref="ArgumentNullException"/>
    public static ITestingServiceBuilder AddMvvm(this ITestingServiceBuilder builder, Action<IMvvmServiceBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddLexicomMvvm(configure);
        builder.Services.AddSingleton<IMessengerScheduler, TestMessengerScheduler>();

        return builder;
    }
}
