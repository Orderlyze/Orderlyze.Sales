using System.Threading;
using System.Threading.Tasks;
using Shiny.Mediator;
using UnoApp.Mediator.Requests.Authentication;

namespace UnoApp.Mediator.Handlers.Authentication;

public class GetStoredTokenHandler : IRequestHandler<GetStoredTokenRequest, TokenData?>
{
    public Task<TokenData?> Handle(GetStoredTokenRequest request, CancellationToken cancellationToken)
    {
        // This will never be called if the cache has a value
        // Return null to indicate no stored token
        return Task.FromResult<TokenData?>(null);
    }
}