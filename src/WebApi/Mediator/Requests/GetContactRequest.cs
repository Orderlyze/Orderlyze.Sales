using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests
{
    public class GetContactRequest : IRequest<ContactDto>
    {
        public Guid Id { get; set; }
    }
}
