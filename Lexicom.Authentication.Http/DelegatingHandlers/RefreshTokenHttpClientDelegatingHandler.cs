using System.Net;

namespace Lexicom.Authentication.Http.DelegatingHandlers;
public class RefreshTokenHttpClientDelegatingHandler : DelegatingHandler
{
    private readonly IHttpClientAccessTokenProvider _httpClientAccessTokenProvider;
    private readonly IHttpClientRefreshTokenProvider _httpClientRefreshTokenProvider;
    private readonly IHttpClientAccessTokenRefresher _httpClientRefreshService;

    /// <exception cref="ArgumentNullException"/>
    public RefreshTokenHttpClientDelegatingHandler(
        IHttpClientAccessTokenProvider httpClientAccessTokenProvider,
        IHttpClientRefreshTokenProvider httpClientRefreshTokenProvider,
        IHttpClientAccessTokenRefresher httpClientRefreshService)
    {
        ArgumentNullException.ThrowIfNull(httpClientAccessTokenProvider);
        ArgumentNullException.ThrowIfNull(httpClientRefreshTokenProvider);
        ArgumentNullException.ThrowIfNull(httpClientRefreshService);

        _httpClientAccessTokenProvider = httpClientAccessTokenProvider;
        _httpClientRefreshTokenProvider = httpClientRefreshTokenProvider;
        _httpClientRefreshService = httpClientRefreshService;

        RefreshSemaphore = new SemaphoreSlim(1, 1);
    }

    protected SemaphoreSlim RefreshSemaphore { get; }

    /// <exception cref="ArgumentNullException"/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return await SendAsync(request, isRefreshed: false, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, bool isRefreshed, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode is not HttpStatusCode.Unauthorized || isRefreshed)
        {
            return response;
        }

        //if the access token has changed by the time we acquire the lock then another 
        //concurrent request already performed the refresh and we must not refresh again
        string? accessTokenBeforeRefresh = await _httpClientAccessTokenProvider.GetAccessTokenAsync();

        await RefreshSemaphore.WaitAsync(cancellationToken);
        try
        {
            string? currentAccessToken = await _httpClientAccessTokenProvider.GetAccessTokenAsync();

            if (currentAccessToken == accessTokenBeforeRefresh)
            {
                string? refreshToken = await _httpClientRefreshTokenProvider.GetRefreshTokenAsync();

                await _httpClientRefreshService.RefreshAuthenticationAsync(currentAccessToken, refreshToken);
            }
        }
        finally
        {
            RefreshSemaphore.Release();
        }

        response.Dispose();

        return await SendAsync(request, isRefreshed: true, cancellationToken);
    }
}
