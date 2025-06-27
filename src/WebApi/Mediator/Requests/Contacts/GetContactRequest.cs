using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    public class GetContactRequest : IRequest<ContactDto>
    {
        public Guid Id { get; set; }
    }
}
