using System.Threading;
using System.Threading.Tasks;
using Shiny.Mediator;
using Shiny.Mediator.Infrastructure;
using UnoApp.Mediator.Requests.Authentication;

namespace UnoApp.Mediator.Handlers.Authentication;

public class ClearTokenHandler : ICommandHandler<ClearTokenCommand>
{
    private readonly ICacheService _cacheService;

    public ClearTokenHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(ClearTokenCommand command, IMediatorContext context, CancellationToken cancellationToken)
    {
        await _cacheService.Remove($"{typeof(GetStoredTokenRequest).FullName}");
    }
}