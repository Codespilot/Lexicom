using Lexicom.Authentication.Http.DelegatingHandlers;
using Lexicom.Authentication.Http.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lexicom.Authentication.Http;
/// <exception cref="ArgumentNullException"/>
public class AuthenticationHttpClientBuilder(IHttpClientBuilder httpClientBuilder)
{
    public IHttpClientBuilder Builder { get; } = httpClientBuilder;

    private bool IncludeAccessTokenHttpClientDelegatingHandler { get; set; }
    private bool IncludeRefreshTokenHttpClientDelegatingHandler { get; set; }
    private bool IncludeUnauthorizedHttpClientDelegatingHandler { get; set; }

    public void AuthorizeWithAccessToken() => IncludeAccessTokenHttpClientDelegatingHandler = true;
    public void AutomaticallyRefreshAccessToken() => IncludeRefreshTokenHttpClientDelegatingHandler = true;
    public void ForwardUnauthorizedRequests() => IncludeUnauthorizedHttpClientDelegatingHandler = true;

    public void Build()
    {
        //the order here matters
        //which is why we use
        //the include booleans

        if (IncludeUnauthorizedHttpClientDelegatingHandler)
        {
            Builder.Services.TryAddTransient<UnauthorizedHttpClientDelegatingHandler>();

            Builder.AddHttpMessageHandler<UnauthorizedHttpClientDelegatingHandler>();
        }

        if (IncludeRefreshTokenHttpClientDelegatingHandler)
        {
            Builder.Services.TryAddSingleton<IRefreshTokenService, RefreshTokenService>();
            Builder.Services.TryAddTransient<RefreshTokenHttpClientDelegatingHandler>();

            Builder.AddHttpMessageHandler<RefreshTokenHttpClientDelegatingHandler>();
        }

        if (IncludeAccessTokenHttpClientDelegatingHandler)
        {
            Builder.Services.TryAddTransient<AccessTokenHttpClientDelegatingHandler>();

            Builder.AddHttpMessageHandler<AccessTokenHttpClientDelegatingHandler>();
        }
    }
}
