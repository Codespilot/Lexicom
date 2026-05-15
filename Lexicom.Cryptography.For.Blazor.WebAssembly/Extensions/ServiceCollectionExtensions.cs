using Lexicom.Cryptography.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lexicom.Cryptography.For.Blazor.WebAssembly.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddLexicomCryptographyForBlazor(this IServiceCollection services, Action<ICryptographyServiceBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddLexicomCryptography(configure);

        services.Replace(new ServiceDescriptor(typeof(IAesProvider), typeof(BlazorAesProvider), ServiceLifetime.Singleton));
    }
}
