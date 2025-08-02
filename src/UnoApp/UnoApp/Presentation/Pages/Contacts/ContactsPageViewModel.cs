using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shiny.Mediator;
using UnoApp.ApiClient;
using UnoApp.Constants;
using UnoApp.Navigation;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Views.Contacts;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Pages.Contacts;

internal partial class ContactsPageViewModel : BasePageViewModel
{
    private readonly INavigator _navigator;
    private readonly IMediator _mediator;

    public ContactsPageViewModel(
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
        return [new("List", RegionViewsNames.ContactList, data: Contacts)];
    }

    public IFeed<IEnumerable<ContactsListModel>> Contacts =>
        Feed.Async(async ct =>
        {
            var contacts = await _mediator.Request(new UnoApp.ApiClient.GetAllContactHttpRequest(), ct).ConfigureAwait(false);
            return contacts.Result.Select(x => new ContactsListModel(
                x.Id,
                x.Name,
                x.Email,
                x.Phone,
                x.Industry
            ));
        });
}
