using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    public class GetAllContactsRequest : IRequest<IEnumerable<ContactDto>> { }
}
