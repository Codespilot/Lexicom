using Lexicom.Authentication.Http.Services;
using System.Net;

namespace Lexicom.Authentication.Http.DelegatingHandlers;
public class RefreshTokenHttpClientDelegatingHandler : DelegatingHandler
{
    private readonly IRefreshTokenService _refreshTokenService;

    /// <exception cref="ArgumentNullException"/>
    public RefreshTokenHttpClientDelegatingHandler(IRefreshTokenService refreshTokenService)
    {
        ArgumentNullException.ThrowIfNull(refreshTokenService);

        _refreshTokenService = refreshTokenService;
    }

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

        await _refreshTokenService.RefreshTokenAsync(cancellationToken);

        response.Dispose();

        return await SendAsync(request, isRefreshed: true, cancellationToken);
    }
}
