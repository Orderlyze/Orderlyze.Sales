using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests
{
    public class GetAllContactsRequest : IRequest<IEnumerable<ContactDto>>
    {
    }
}
