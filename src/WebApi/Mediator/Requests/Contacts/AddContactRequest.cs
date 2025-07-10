using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    internal class AddContactRequest : IRequest<ContactDto>
    {
        public string WixId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Branche { get; set; } = "";
        public bool SetInitialCallDate { get; set; } = true; // Set NextCallDate to today by default
    }
}
