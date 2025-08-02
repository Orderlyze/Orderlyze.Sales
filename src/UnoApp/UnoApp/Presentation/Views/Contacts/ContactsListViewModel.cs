using System.Collections.Generic;
using System.Threading.Tasks;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Services.Common;
using Microsoft.UI.Xaml;

namespace UnoApp.Presentation.Views.Contacts;

internal partial class ContactsListViewModel : BaseItemViewModel<IEnumerable<ContactsListModel>>
{
    public ContactsListViewModel(
        BaseServices services,
        IFeed<IEnumerable<ContactsListModel>> contacts
    ) : base(services, contacts) 
    {
    }

    public override Task InitializeAsync(RoutedEventArgs e)
    {
        return base.InitializeAsync(e);
    }
}