using System;
using Shiny.Mediator;

namespace UnoApp.Services.Authentication;

public record TokenData(
    string AccessToken, 
    string RefreshToken, 
    DateTime ExpiresAt
);

[Cache(AbsoluteExpirationSeconds = 86400 * 30, StorageType = CacheStorageType.Persistent)] // 30 days
public record GetStoredTokenRequest : IRequest<TokenData?>
{
    public const string CacheKey = "auth_token";
}

public class GetStoredTokenRequestHandler : IRequestHandler<GetStoredTokenRequest, TokenData?>
{
    public Task<TokenData?> Handle(GetStoredTokenRequest request, CancellationToken cancellationToken)
    {
        // This will never be called if the cache has a value
        // Return null to indicate no stored token
        return Task.FromResult<TokenData?>(null);
    }
}

public record SaveTokenRequest : IRequest
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
}

public class SaveTokenRequestHandler : IRequestHandler<SaveTokenRequest>
{
    private readonly IMediator _mediator;

    public SaveTokenRequestHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(SaveTokenRequest request, CancellationToken cancellationToken)
    {
        var tokenData = new TokenData(
            request.AccessToken,
            request.RefreshToken,
            request.ExpiresAt
        );

        // Store in cache by making a request with CacheSet
        await _mediator.CacheSet(
            GetStoredTokenRequest.CacheKey,
            tokenData,
            absoluteExpiration: DateTimeOffset.UtcNow.AddDays(30)
        );
    }
}

public record ClearTokenRequest : IRequest;

public class ClearTokenRequestHandler : IRequestHandler<ClearTokenRequest>
{
    private readonly IMediator _mediator;

    public ClearTokenRequestHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(ClearTokenRequest request, CancellationToken cancellationToken)
    {
        await _mediator.CacheRemove(GetStoredTokenRequest.CacheKey);
    }
}