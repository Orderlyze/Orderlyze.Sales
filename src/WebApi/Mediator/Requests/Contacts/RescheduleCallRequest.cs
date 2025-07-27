using SharedModels.Dtos.Contacts;

namespace WebApi.Mediator.Requests.Contacts
{
    internal class RescheduleCallRequest : IRequest<ContactDto>
    {
        public int ContactId { get; set; }
        public DateTime? NewCallDate { get; set; } // If null, use user's default days
        public string? Reason { get; set; }
    }
}