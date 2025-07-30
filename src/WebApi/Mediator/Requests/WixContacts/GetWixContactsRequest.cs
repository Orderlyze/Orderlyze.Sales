using System.ComponentModel.DataAnnotations;
using Shiny.Mediator;

namespace WebApi.Mediator.Requests.WixContacts;

/// <summary>
/// Request to get contacts from Wix API.
/// </summary>
public class GetWixContactsRequest : IRequest<List<WixContactDto>>
{
    /// <summary>
    /// Gets or sets the maximum number of contacts to retrieve.
    /// </summary>
    [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100")]
    public int Limit { get; set; } = 20;
}