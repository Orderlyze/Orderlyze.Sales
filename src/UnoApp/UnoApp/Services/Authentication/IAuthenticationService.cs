using System;
using System.Threading.Tasks;

namespace UnoApp.Services.Authentication;

public interface IAuthenticationService
{
    string? AccessToken { get; }
    string? RefreshToken { get; }
    DateTime? TokenExpiresAt { get; }
    bool IsAuthenticated { get; }
    
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RefreshTokenAsync();
    Task LogoutAsync();
    Task<string?> GetValidTokenAsync();
}