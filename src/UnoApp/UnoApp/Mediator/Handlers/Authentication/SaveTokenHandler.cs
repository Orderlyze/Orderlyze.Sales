using System;
using System.Threading;
using System.Threading.Tasks;
using Shiny.Mediator;
using Shiny.Mediator.Infrastructure;
using UnoApp.Mediator.Requests.Authentication;

namespace UnoApp.Mediator.Handlers.Authentication;

public class SaveTokenHandler : ICommandHandler<SaveTokenCommand>
{
    private readonly ICacheService _cacheService;

    public SaveTokenHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(SaveTokenCommand command, IMediatorContext context, CancellationToken cancellationToken)
    {
        var tokenData = new TokenData(
            command.AccessToken,
            command.RefreshToken,
            command.ExpiresAt
        );

        // Store in cache using the key from configuration
        await _cacheService.Set(
            $"{typeof(GetStoredTokenRequest).FullName}",
            tokenData,
            new CacheItemConfig(AbsoluteExpiration: TimeSpan.FromDays(30))
        );
    }
}