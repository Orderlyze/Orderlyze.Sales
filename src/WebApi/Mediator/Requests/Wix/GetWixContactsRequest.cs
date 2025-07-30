using Shiny.Mediator;
using SharedModels.Dtos.Wix;

namespace WebApi.Mediator.Requests.Wix
{
    public record GetWixContactsRequest : IRequest<IEnumerable<WixContactDto>>;
}