using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    internal class GetContactRequest : IRequest<ContactDto>
    {
        public Guid Id { get; set; }
    }
}
