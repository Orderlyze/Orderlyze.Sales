using System;
using Shiny.Mediator;

namespace UnoApp.Mediator.Requests.Authentication;

public record TokenData(
    string AccessToken, 
    string RefreshToken, 
    DateTime ExpiresAt
);

[Cache]
public record GetStoredTokenRequest : IRequest<TokenData?>;