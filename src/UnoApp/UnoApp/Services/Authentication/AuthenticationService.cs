using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Mediator;
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
        Task.Run(async () => await RestoreTokensAsync());
    }

    public string? AccessToken => _accessToken;
    public string? RefreshToken => _refreshToken;
    public DateTime? TokenExpiresAt => _tokenExpiresAt;
    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken) && _tokenExpiresAt > DateTime.UtcNow;

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var result = await _mediator.Request(new LoginHttpRequest
            {
                Body = new LoginRequest
                {
                    Email = email,
                    Password = password
                }
            });

            if (result.Result != null)
            {
                StoreTokens(result.Result);
                return true;
            }

            _logger.LogWarning("Login failed");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error");
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
            var result = await _mediator.Request(new RefreshHttpRequest
            {
                Body = new RefreshRequest
                {
                    RefreshToken = _refreshToken
                }
            });

            if (result.Result != null)
            {
                StoreTokens(result.Result);
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
        await _mediator.Send(new ClearTokenCommand());
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
            var refreshed = await RefreshTokenAsync();
            if (refreshed)
            {
                return _accessToken;
            }
        }

        return null;
    }

    private async void StoreTokens(AccessTokenResponse tokenResponse)
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
            });
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
            var result = await _mediator.Request(new GetStoredTokenRequest());
            if (result.Result != null)
            {
                _accessToken = result.Result.AccessToken;
                _refreshToken = result.Result.RefreshToken;
                _tokenExpiresAt = result.Result.ExpiresAt;
                
                _logger.LogInformation("Tokens restored from storage, expires at: {ExpiresAt}", _tokenExpiresAt);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore tokens from storage");
        }
    }
}