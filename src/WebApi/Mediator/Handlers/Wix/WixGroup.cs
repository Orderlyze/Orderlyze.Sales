using Microsoft.AspNetCore.Http;
using SharedModels.Dtos.Wix;
using Shiny.Mediator;
using WixApi.Repositories;
using WebApi.Constants;
using WebApi.Mediator.Requests.Wix;
using System.Linq;

namespace WebApi.Mediator.Handlers.Wix
{
    /// <summary>
    /// Handles all Wix-related operations.
    /// </summary>
    [MediatorHttpGroup("wix", RequiresAuthorization = true)]
    internal sealed class WixGroup(IWixContactsRepository wixContactsRepository)
        : IRequestHandler<GetWixContactsRequest, IEnumerable<WixContactDto>>
    {
        /// <summary>
        /// Retrieves all contacts from Wix.
        /// </summary>
        /// <param name="request">The request to get Wix contacts.</param>
        /// <param name="context">The mediator context.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A collection of Wix contact DTOs.</returns>
        [MediatorHttpGet("contacts", GroupConstants.NoTemplate)]
        public async Task<IEnumerable<WixContactDto>> Handle(
            GetWixContactsRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            var wixContacts = await wixContactsRepository.GetContactsAsync();
            
            return wixContacts.Select(x => new WixContactDto(
                x.id,
                x.info.name?.first,
                x.info.name?.last,
                x.primaryInfo.email,
                x.primaryInfo.phone ?? x.info.phones?.items?.FirstOrDefault()?.phone,
                x.info.addresses?.items?.FirstOrDefault()?.address?.addressLine,
                x.info.company,
                x.info.labelKeys?.items?.ToArray() ?? Array.Empty<string>()
            ));
        }
    }
}