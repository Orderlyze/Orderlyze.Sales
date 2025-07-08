using SharedModels.Dtos.Contacts;
using UnoApp.Mediator.Requests.Contacts;

namespace UnoApp.Mediator.Handlers.Contacts;

[SingletonHandler]
[MediatorHttpPost("/api/contacts")]
internal class AddContactRequestHandler : IRequestHandler<AddContactRequest, ContactDto>
{
    public async ValueTask<ContactDto> Handle(AddContactRequest request, CancellationToken cancellationToken)
    {
        // This will be handled by the generated HTTP client through Shiny.Mediator
        // The actual HTTP POST will be performed by the source generator
        await Task.CompletedTask;
        return new ContactDto();
    }
}