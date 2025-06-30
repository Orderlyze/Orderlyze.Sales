using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Services.Common;
using WixApi.Models;

namespace UnoApp.Presentation.Views.WixContacts;

internal partial class WixContactsListViewModel
    : BaseItemViewModel<IEnumerable<WixContactsListModel>>
{
    public WixContactsListViewModel(
        BaseServices services,
        IFeed<IEnumerable<WixContactsListModel>> wixContacts
    )
        : base(services, wixContacts) { }

    public override Task InitializeAsync(RoutedEventArgs e)
    {
        return base.InitializeAsync(e);
    }
}
