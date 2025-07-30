using Shiny.Mediator;

namespace WebApi.Mediator.Requests.WixContacts;

public class GetWixContactsRequest : IRequest<List<WixContactDto>>
{
    public int Limit { get; set; } = 20;
}