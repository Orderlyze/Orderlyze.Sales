using Shiny.Mediator;
using SharedModels.Dtos.Contacts;
using System.Collections.Generic;

namespace UnoApp.Mediator.Requests.Contacts;

public record GetAllContactsRequest : IRequest<IEnumerable<ContactDto>>;