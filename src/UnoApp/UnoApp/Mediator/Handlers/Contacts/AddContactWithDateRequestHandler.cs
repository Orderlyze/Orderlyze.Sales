using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Mediator;
using UnoApp.Mediator.Requests.Contacts;

namespace UnoApp.Mediator.Handlers.Contacts;

internal class AddContactWithDateRequestHandler : IRequestHandler<AddContactWithDateRequest, string>
{
    public Task<string> Handle(
        AddContactWithDateRequest request,
        IMediatorContext context,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
