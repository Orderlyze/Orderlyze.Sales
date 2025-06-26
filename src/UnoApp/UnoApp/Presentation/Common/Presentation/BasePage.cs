using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Navigation;

namespace UnoApp.Presentation.Common.Presentation;

public partial class BasePage : Page
{
    private NavigationEventArgs? _pendingNavigationArgs;

    public BasePage()
    {
        this.DataContextChanged += OnDataContextChanged;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (this.DataContext is INavigationAware<NavigationEventArgs> aware)
        {
            aware.OnNavigatedTo(e);
        }
        else
        {
            // DataContext noch nicht gesetzt → auf später verschieben
            _pendingNavigationArgs = e;
        }
    }

    private void OnDataContextChanged(FrameworkElement sender, object args)
    {
        if (
            _pendingNavigationArgs is not null
            && this.DataContext is INavigationAware<NavigationEventArgs> aware
        )
        {
            aware.OnNavigatedTo(_pendingNavigationArgs);
            _pendingNavigationArgs = null;
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        if (DataContext is INavigationAware<NavigationEventArgs> aware)
        {
            aware.OnNavigatedFrom(e);
        }
    }
}
