using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Constants;
using UnoApp.Navigation;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Views.WixContacts;
using UnoApp.Services.Common;
using WixApi.Models;
using WixApi.Repositories;

namespace UnoApp.Presentation.Pages.WixContacts;

internal partial class WixContactsPageViewModel : BasePageViewModel
{
    private readonly INavigator navigator;
    private readonly IWixContactsRepository wixContactsRepository;

    public WixContactsPageViewModel(
        BaseServices baseServices,
        INavigator navigator,
        IWixContactsRepository wixContactsRepository
    )
        : base(baseServices)
    {
        this.navigator = navigator;
        this.wixContactsRepository = wixContactsRepository;
    }

    public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
    {
        return [new("List", RegionViewsNames.WixContactList, data: WixContacts)];
    }

    public IFeed<IEnumerable<WixContactsListModel>> WixContacts =>
        Feed.Async(async ct =>
        {
            var wixContacts = await wixContactsRepository.GetContactsAsync();
            return wixContacts.Select(x => new WixContactsListModel(
                x.id,
                x.info.name?.first + " " + x.info.name?.last,
                x.primaryInfo.email,
                x.primaryInfo.phone ?? x.info.phones?.items?.FirstOrDefault()?.phone,
                x.info.addresses?.items?.FirstOrDefault()?.address?.addressLine, // Branche - using first address line as branch
                x.info.company,
                x.info.labelKeys?.items?.ToArray() ?? Array.Empty<string>()
            ));
        });
}
