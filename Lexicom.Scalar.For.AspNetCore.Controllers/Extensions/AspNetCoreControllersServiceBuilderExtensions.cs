using Lexicom.Supports.AspNetCore.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Lexicom.Scalar.Extensions;
public static class AspNetCoreControllersServiceBuilderExtensions
{
    /// <exception cref="ArgumentNullException"/>
    public static IAspNetCoreControllersServiceBuilder AddScalar(this IAspNetCoreControllersServiceBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return AddScalar(builder, settings: null);
    }
    /// <exception cref="ArgumentNullException"/>
    public static IAspNetCoreControllersServiceBuilder AddScalar(this IAspNetCoreControllersServiceBuilder builder, ScalarSettings? settings)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddOpenApi(options =>
        {

        });

        return builder;
    }
}
