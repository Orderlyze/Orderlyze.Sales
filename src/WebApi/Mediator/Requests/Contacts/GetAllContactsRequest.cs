using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    internal class GetAllContactsRequest : IRequest<IEnumerable<ContactDto>> { }
}
