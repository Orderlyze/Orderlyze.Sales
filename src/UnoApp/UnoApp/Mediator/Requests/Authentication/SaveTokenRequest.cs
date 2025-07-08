using System;
using Shiny.Mediator;

namespace UnoApp.Mediator.Requests.Authentication;

public record SaveTokenRequest : IRequest
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
}