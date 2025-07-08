using System;
using System.Threading;
using System.Threading.Tasks;
using Shiny.Mediator;
using UnoApp.Mediator.Requests.Authentication;

namespace UnoApp.Mediator.Handlers.Authentication;

public class SaveTokenHandler : IRequestHandler<SaveTokenRequest>
{
    private readonly IMediator _mediator;

    public SaveTokenHandler(IMediator mediator)
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

        // Store in cache using the key from configuration
        await _mediator.CacheSet(
            "GetStoredTokenRequest",
            tokenData,
            absoluteExpiration: DateTimeOffset.UtcNow.AddDays(30)
        );
    }
}