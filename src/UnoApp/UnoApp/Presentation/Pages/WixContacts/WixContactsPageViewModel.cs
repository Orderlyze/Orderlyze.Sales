using System;
using System.Collections.Generic;
using System.Linq;
using Shiny.Mediator;
using UnoApp.ApiClient;
using UnoApp.Constants;
using UnoApp.Navigation;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Views.WixContacts;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Pages.WixContacts;

internal partial class WixContactsPageViewModel : BasePageViewModel
{
    private readonly INavigator _navigator;
    private readonly IMediator _mediator;

    public WixContactsPageViewModel(
        BaseServices baseServices,
        INavigator navigator,
        IMediator mediator
    )
        : base(baseServices)
    {
        _navigator = navigator;
        _mediator = mediator;
    }

    public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
    {
        return [new("List", RegionViewsNames.WixContactList, data: WixContacts)];
    }

    public IFeed<IEnumerable<WixContactsListModel>> WixContacts =>
        Feed.Async(async ct =>
        {
            try
            {
                var response = await _mediator.Request(new ContactsHttpRequest(), ct);
                
                if (response.Result != null)
                {
                    return response.Result.Select(x => new WixContactsListModel(
                        x.Id,
                        $"{x.FirstName} {x.LastName}".Trim(),
                        x.Email,
                        x.Phone,
                        x.Address, // Branche - using address as branch
                        x.Company,
                        x.LabelKeys?.ToArray() ?? Array.Empty<string>()
                    ));
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Console.WriteLine($"Error fetching Wix contacts: {ex.Message}");
            }
            
            return Enumerable.Empty<WixContactsListModel>();
        });
}