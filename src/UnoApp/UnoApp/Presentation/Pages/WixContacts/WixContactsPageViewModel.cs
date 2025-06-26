using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Pages.WixDetail;
using UnoApp.Services.Common;
using WixApi.Models;
using WixApi.Repositories;

namespace UnoApp.Presentation.Pages.WixContacts;

internal partial class WixContactsPageViewModel(
    BaseServices baseServices,
    IWixContactsRepository wixContactsRepository
) : BasePageViewModel(baseServices)
{
    List<WixContact> WixContacts = new List<WixContact>();

    public override async Task InitializeAsync(NavigationEventArgs e)
    {
        await base.InitializeAsync(e);
        WixContacts = await wixContactsRepository.GetContactsAsync();
    }
}
