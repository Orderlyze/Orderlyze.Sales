using System.Threading;
using System.Threading.Tasks;
using Shiny.Mediator;
using UnoApp.Mediator.Requests.Authentication;

namespace UnoApp.Mediator.Handlers.Authentication;

public class ClearTokenHandler : IRequestHandler<ClearTokenRequest>
{
    private readonly IMediator _mediator;

    public ClearTokenHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(ClearTokenRequest request, CancellationToken cancellationToken)
    {
        await _mediator.CacheRemove("GetStoredTokenRequest");
    }
}