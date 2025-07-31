using Shiny.Mediator;
using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    public record ImportWixContactRequest(string WixContactId) : IRequest<ContactDto>;
}