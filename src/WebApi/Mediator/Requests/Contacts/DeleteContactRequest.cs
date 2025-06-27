namespace WebApi.Mediator.Requests.Contacts
{
    public class DeleteContactRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
