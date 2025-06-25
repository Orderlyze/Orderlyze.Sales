using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Pages.WixDetail;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Pages.WixContacts;

internal class WixContactsPageViewModel : BasePageViewModel
{
    private readonly INavigator navigator;

    public WixContactsPageViewModel(INavigator navigator, BaseServices baseServices)
        : base(baseServices)
    {
        this.navigator = navigator;
        Test();
    }

    private async Task Test()
    {
        await navigator.NavigateViewAsync<WixDetailPage>(this);
    }
}
