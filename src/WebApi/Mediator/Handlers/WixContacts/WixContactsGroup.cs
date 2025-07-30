using Microsoft.AspNetCore.Http;
using Shiny.Mediator;
using WebApi.Constants;
using WebApi.Mediator.Requests.WixContacts;
using WixApi.Repositories;

namespace WebApi.Mediator.Handlers.WixContacts;

[SingletonHandler]
internal class WixContactsGroup(
    IWixContactsRepository wixContactsRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetWixContactsRequest, List<WixContactDto>>
{
    [MediatorHttpGet("/WixContacts", "", RequiresAuthorization = true)]
    public async Task<List<WixContactDto>> Handle(GetWixContactsRequest request, IMediatorContext context, CancellationToken ct)
    {
        var wixContacts = await wixContactsRepository.GetContactsAsync(request.Limit);
        
        return wixContacts.Select(x => new WixContactDto
        {
            Id = x.id,
            FirstName = x.info.name?.first,
            LastName = x.info.name?.last,
            FullName = $"{x.info.name?.first} {x.info.name?.last}".Trim(),
            Email = x.primaryInfo.email,
            Phone = x.primaryInfo.phone ?? x.info.phones?.items?.FirstOrDefault()?.phone,
            Company = x.info.company,
            Labels = x.info.labelKeys?.items?.ToList() ?? new List<string>(),
            CreatedDate = x.createdDate,
            UpdatedDate = x.updatedDate
        }).ToList();
    }
}