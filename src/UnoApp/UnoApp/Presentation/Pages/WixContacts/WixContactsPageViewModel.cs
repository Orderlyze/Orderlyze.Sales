using System;
using System.Collections.Generic;
using System.Linq;
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
            var request = new GetWixContactsHttpRequest
            {
                Limit = 20
            };
            
            var response = await _mediator.Request(request, ct);
            return response.Result.Select(x => new WixContactsListModel(
                x.Id ?? string.Empty,
                x.FullName ?? $"{x.FirstName} {x.LastName}".Trim(),
                x.Email,
                x.Phone,
                x.Company, // Using company as branch field
                x.Company,
                x.Labels?.ToArray() ?? Array.Empty<string>()
            ));
        });
}
