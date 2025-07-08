using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly TimeSpan _tokenExpiration = TimeSpan.FromHours(1);
    private readonly TimeSpan _refreshTokenExpiration = TimeSpan.FromDays(14);

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Enhanced login endpoint that returns token expiration as DateTime
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await _signInManager.PasswordSignInAsync(
            loginRequest.Email, 
            loginRequest.Password, 
            isPersistent: false, 
            lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(loginRequest.TwoFactorCode))
            {
                result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                    loginRequest.TwoFactorCode, 
                    isPersistent: false, 
                    rememberClient: false);
            }
            else if (!string.IsNullOrEmpty(loginRequest.TwoFactorRecoveryCode))
            {
                result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(
                    loginRequest.TwoFactorRecoveryCode);
            }
            else
            {
                return Problem("Two factor authentication is required.", statusCode: 401);
            }
        }

        if (!result.Succeeded)
        {
            return Problem(result.IsLockedOut ? "User is locked out." : "Invalid login attempt.", statusCode: 401);
        }

        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            return Problem("User not found.", statusCode: 404);
        }

        // Generate token with expiration info
        var currentTime = DateTime.UtcNow;
        var expiresAt = currentTime.Add(_tokenExpiration);

        return Ok(new ExtendedAccessTokenResponse
        {
            TokenType = "Bearer",
            AccessToken = GenerateAccessToken(user.Id),
            ExpiresIn = (long)_tokenExpiration.TotalSeconds,
            ExpiresAt = expiresAt,
            RefreshToken = GenerateRefreshToken()
        });
    }

    /// <summary>
    /// Enhanced refresh endpoint that returns token expiration as DateTime
    /// </summary>
    [HttpPost("refresh")]
    public Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        // This is a simplified implementation
        // In production, you would validate the refresh token from a store
        // and get the associated user
        
        var currentTime = DateTime.UtcNow;
        var expiresAt = currentTime.Add(_tokenExpiration);

        // For demo purposes, we're creating a new token
        // In production, validate the refresh token and get the user
        return Task.FromResult<IActionResult>(Ok(new ExtendedAccessTokenResponse
        {
            TokenType = "Bearer",
            AccessToken = GenerateAccessToken(Guid.NewGuid().ToString()),
            ExpiresIn = (long)_tokenExpiration.TotalSeconds,
            ExpiresAt = expiresAt,
            RefreshToken = GenerateRefreshToken()
        }));
    }

    private string GenerateAccessToken(string userId)
    {
        // This is a placeholder - in production, use proper JWT generation
        // with claims, signing, etc.
        return $"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.{Convert.ToBase64String(Guid.NewGuid().ToByteArray())}";
    }

    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}