using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Mediator;

namespace UnoApp.Mediator.Requests.Contacts;

internal record AddContactWithDateRequest : IRequest<string> { }
