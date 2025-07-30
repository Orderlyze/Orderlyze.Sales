using Shiny.Mediator;
using WebApi.Constants;
using WebApi.Mediator.Requests.WixContacts;
using WixApi.Models;
using WixApi.Repositories;

namespace WebApi.Mediator.Handlers.WixContacts;

/// <summary>
/// Handles Wix contacts proxy operations.
/// </summary>
[MediatorHttpGroup("WixContacts", RequiresAuthorization = true)]
internal sealed class WixContactsGroup(IWixContactsRepository wixContactsRepository)
    : IRequestHandler<GetWixContactsRequest, List<WixContactDto>>
{
    /// <summary>
    /// Gets contacts from Wix API.
    /// </summary>
    /// <param name="request">The request with limit parameter.</param>
    /// <param name="context">The mediator context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of Wix contacts.</returns>
    [MediatorHttpGet("getWixContacts", "")]
    public async Task<List<WixContactDto>> Handle(
        GetWixContactsRequest request,
        IMediatorContext context,
        CancellationToken ct)
    {
        var wixContacts = await wixContactsRepository.GetContactsAsync(request.Limit);
        
        return wixContacts.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Maps a WixContact to WixContactDto.
    /// </summary>
    /// <param name="contact">The Wix contact to map.</param>
    /// <returns>The mapped DTO.</returns>
    private static WixContactDto MapToDto(WixContact contact)
    {
        return new WixContactDto
        {
            Id = contact.id ?? string.Empty,
            FirstName = contact.info?.name?.first,
            LastName = contact.info?.name?.last,
            FullName = $"{contact.info?.name?.first} {contact.info?.name?.last}".Trim(),
            Email = contact.primaryInfo?.email,
            Phone = contact.primaryInfo?.phone ?? contact.info?.phones?.items?.FirstOrDefault()?.phone,
            Company = contact.info?.company,
            Labels = contact.info?.labelKeys?.items?.ToList() ?? new List<string>(),
            CreatedDate = contact.createdDate,
            UpdatedDate = contact.updatedDate
        };
    }
}