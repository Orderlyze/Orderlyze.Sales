using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using UnoApp.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnoApp.Presentation.Common;

public partial class BaseView
{
    private RoutedEventArgs? _pendingNavigationArgs;

    public BaseView()
    {
        this.InitializeComponent();
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
