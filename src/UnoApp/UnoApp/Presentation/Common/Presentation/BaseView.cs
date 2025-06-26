using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Navigation;

namespace UnoApp.Presentation.Common.Presentation;

public class BaseView : UserControl
{
    private RoutedEventArgs? _pendingNavigationArgs;

    public BaseView()
    {
        this.DataContextChanged += OnDataContextChanged;
        this.Loaded += BaseView_Loaded;
        this.Unloaded += BaseView_Unloaded;
    }

    private void BaseView_Unloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is INavigationAware<RoutedEventArgs> aware)
        {
            aware.OnNavigatedFrom(e);
        }
    }

    private void BaseView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is INavigationAware<RoutedEventArgs> aware)
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
            && this.DataContext is INavigationAware<RoutedEventArgs> aware
        )
        {
            aware.OnNavigatedTo(_pendingNavigationArgs);
            _pendingNavigationArgs = null;
        }
    }
}
