using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shiny.Mediator.Http;
using WixApi.Models;

namespace WixApi.Mediator.Requests
{
    public record ListContactsRequest : IRequest<List<WixContact>> { }
}
