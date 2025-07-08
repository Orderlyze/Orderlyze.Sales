using System;

namespace WebApi.Models;

public class ExtendedAccessTokenResponse
{
    public string? TokenType { get; set; }
    public required string AccessToken { get; set; }
    public long ExpiresIn { get; set; }
    public DateTime ExpiresAt { get; set; }
    public required string RefreshToken { get; set; }
}