using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WixApi.Mediator.Requests
{
    public record GetContactLabelsRequest(string ContactId) : IRequest<object>;
}
