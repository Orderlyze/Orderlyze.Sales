namespace WebApi.Mediator.Requests.Contacts
{
    internal class DeleteContactRequest : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
