using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.Client.Contacts.V4.Contacts;
using MyApi.Client.Models;
using Shiny.Mediator.Http;
using WixApi.Models;

namespace WixApi.Mediator.Requests
{
    public record ListContactsRequest(int Limit = 20) : IRequest<ContactList> { }
}
