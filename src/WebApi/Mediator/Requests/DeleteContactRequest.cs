namespace WebApi.Mediator.Requests
{
    public class DeleteContactRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}
