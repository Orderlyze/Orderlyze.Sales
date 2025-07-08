using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    internal class AddContactRequest : IRequest<ContactDto>
    {
        public string WixId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Branche { get; set; } = string.Empty;
    }
}
