using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Mediator;
using Shiny.Mediator.Http;
using UnoApp.ApiClient;
using UnoApp.Mediator.Requests.Authentication;

namespace UnoApp.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private const int TokenExpirationBufferSeconds = 60;
    
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticationService> _logger;
    
    private string? _accessToken;
    private string? _refreshToken;
    private DateTime? _tokenExpiresAt;

    public AuthenticationService(IMediator mediator, ILogger<AuthenticationService> logger)
    {
        _mediator = mediator;
        _logger = logger;
        
        // Try to restore tokens from persistent storage
        Task.Run(async () => 
        {
            try
            {
                await RestoreTokensAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to restore tokens from storage");
            }
        });
    }

    public string? AccessToken => _accessToken;
    public string? RefreshToken => _refreshToken;
    public DateTime? TokenExpiresAt => _tokenExpiresAt;
    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken) && _tokenExpiresAt > DateTime.UtcNow;

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            _logger.LogInformation("Starting login attempt for {Email}", email);
            
            var loginRequest = new LoginHttpRequest
            {
                Body = new LoginRequest
                {
                    Email = email,
                    Password = password
                }
            };
            
            _logger.LogDebug("Sending login request to API");
            
            var (context, result) = await _mediator.Request(loginRequest).ConfigureAwait(false);
            
            _logger.LogInformation("API response received, Has result: {HasResult}", 
                result != null);

            if (result != null)
            {
                _logger.LogInformation("Login successful, received tokens");
                await StoreTokens(result).ConfigureAwait(false);
                return true;
            }

            _logger.LogWarning("Login failed - no result returned from API");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error for {Email}", email);
            return false;
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        if (string.IsNullOrEmpty(_refreshToken))
        {
            _logger.LogWarning("No refresh token available");
            return false;
        }

        try
        {
            var (context, result) = await _mediator.Request(new RefreshHttpRequest
            {
                Body = new RefreshRequest
                {
                    RefreshToken = _refreshToken
                }
            }).ConfigureAwait(false);

            if (result != null)
            {
                await StoreTokens(result).ConfigureAwait(false);
                return true;
            }

            _logger.LogWarning("Token refresh failed");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token refresh error");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        _accessToken = null;
        _refreshToken = null;
        _tokenExpiresAt = null;
        
        // Clear persistent storage
        await _mediator.Send(new ClearTokenCommand()).ConfigureAwait(false);
    }

    public async Task<string?> GetValidTokenAsync()
    {
        // Check if token is still valid
        if (IsAuthenticated)
        {
            return _accessToken;
        }

        // Try to refresh the token
        if (!string.IsNullOrEmpty(_refreshToken))
        {
            var refreshed = await RefreshTokenAsync().ConfigureAwait(false);
            if (refreshed)
            {
                return _accessToken;
            }
        }

        return null;
    }

    private async Task StoreTokens(AccessTokenResponse tokenResponse)
    {
        _accessToken = tokenResponse.AccessToken;
        _refreshToken = tokenResponse.RefreshToken;
        
        // Calculate expiration time from ExpiresIn
        // Subtract buffer seconds to refresh before actual expiration
        _tokenExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - TokenExpirationBufferSeconds);
        
        _logger.LogInformation("Tokens stored, expires at: {ExpiresAt}", _tokenExpiresAt);
        
        // Persist tokens
        try
        {
            await _mediator.Send(new SaveTokenCommand
            {
                AccessToken = _accessToken,
                RefreshToken = _refreshToken,
                ExpiresAt = _tokenExpiresAt.GetValueOrDefault(DateTime.UtcNow.AddHours(1))
            }).ConfigureAwait(false);
            _logger.LogDebug("Tokens persisted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist tokens");
        }
    }
    
    private async Task RestoreTokensAsync()
    {
        try
        {
            var (context, result) = await _mediator.Request(new GetStoredTokenRequest()).ConfigureAwait(false);
            if (result != null)
            {
                _accessToken = result.AccessToken;
                _refreshToken = result.RefreshToken;
                _tokenExpiresAt = result.ExpiresAt;
                
                _logger.LogInformation("Tokens restored from storage, expires at: {ExpiresAt}", _tokenExpiresAt);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore tokens from storage");
        }
    }
}