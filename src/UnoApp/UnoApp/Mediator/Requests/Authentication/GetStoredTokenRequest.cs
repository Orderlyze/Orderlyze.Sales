using System;
using Shiny.Mediator;

namespace UnoApp.Mediator.Requests.Authentication;

public record TokenData(
    string AccessToken, 
    string RefreshToken, 
    DateTime ExpiresAt
);

public record GetStoredTokenRequest : IRequest<TokenData?>;