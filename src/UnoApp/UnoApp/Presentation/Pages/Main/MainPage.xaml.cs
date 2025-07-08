using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UnoApp.Constants;

namespace UnoApp.Presentation;

public partial class MainPage
{
    public MainPage()
    {
        this.InitializeComponent();
    }

    private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item && DataContext is MainViewModel viewModel)
        {
            var tag = item.Tag?.ToString();
            
            // Hide all content
            OneContent.Visibility = Visibility.Collapsed;
            TwoContent.Visibility = Visibility.Collapsed;
            ThreeContent.Visibility = Visibility.Collapsed;
            DynamicContent.Visibility = Visibility.Collapsed;
            
            switch (tag)
            {
                case "One":
                    OneContent.Visibility = Visibility.Visible;
                    break;
                case "Two":
                    TwoContent.Visibility = Visibility.Visible;
                    break;
                case "Three":
                    ThreeContent.Visibility = Visibility.Visible;
                    break;
                case PageNames.Contacts:
                case PageNames.WixContacts:
                    DynamicContent.Visibility = Visibility.Visible;
                    // Navigate to the selected page through the attached region
                    var navigator = viewModel.GetNavigator();
                    if (navigator != null)
                    {
                        await navigator.NavigateRouteAsync(this, tag);
                    }
                    break;
            }
        }
    }
}
