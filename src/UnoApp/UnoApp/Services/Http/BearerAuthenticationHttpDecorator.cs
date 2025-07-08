using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Mediator.Http;
using UnoApp.Services.Authentication;

namespace UnoApp.Services.Http;

using Authentication = UnoApp.Services.Authentication;

public class BearerAuthenticationHttpDecorator : IHttpRequestDecorator
{
    private readonly Authentication.IAuthenticationService _authService;
    private readonly ILogger<BearerAuthenticationHttpDecorator> _logger;

    public BearerAuthenticationHttpDecorator(
        Authentication.IAuthenticationService authService,
        ILogger<BearerAuthenticationHttpDecorator> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task Decorate(HttpRequestMessage request, IMediatorContext context, CancellationToken cancellationToken)
    {
        // Skip authentication for login and register endpoints
        var path = request.RequestUri?.AbsolutePath ?? "";
        if (path.Contains("/login", StringComparison.OrdinalIgnoreCase) || 
            path.Contains("/register", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogDebug("Skipping authentication for {Path}", path);
            return;
        }

        // Get valid token (will refresh if needed)
        var token = await _authService.GetValidTokenAsync();
        
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _logger.LogDebug("Added Bearer token to request for {Path}", path);
        }
        else
        {
            _logger.LogWarning("No valid token available for {Path}", path);
        }
    }
}