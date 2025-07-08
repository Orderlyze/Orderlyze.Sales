using System;
using Shiny.Mediator;

namespace UnoApp.Mediator.Requests.Authentication;

public record SaveTokenCommand : Shiny.Mediator.ICommand
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
}