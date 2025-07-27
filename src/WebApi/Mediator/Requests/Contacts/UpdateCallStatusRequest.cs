using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    internal class UpdateCallStatusRequest : IRequest<ContactDto>
    {
        public int ContactId { get; set; }
        public CallStatus Status { get; set; }
        public string Notes { get; set; } = "";
        public bool RescheduleCall { get; set; } = false;
        public DateTime? NextCallDate { get; set; }
    }
}