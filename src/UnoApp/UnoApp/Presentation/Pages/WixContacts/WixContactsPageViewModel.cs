using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Presentation.Pages.WixDetail;

namespace UnoApp.Presentation.Pages.WixContacts;

internal class WixContactsPageViewModel : ObservableObject
{
    private readonly INavigator navigator;

    public WixContactsPageViewModel(INavigator navigator)
    {
        this.navigator = navigator;
        Test();
    }

    private async Task Test()
    {
        await navigator.NavigateViewAsync<WixDetailPage>(this);
    }
}
