using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Navigation;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Common;

public partial class BaseViewModel(BaseServices Services) : ObservableObject, INavigationAware
{
    public virtual Task InitializeAsync(NavigationEventArgs e)
    {
        return Task.CompletedTask;
    }

    protected virtual void NavigatedTo(NavigationEventArgs e)
    {
        // Default no-op; derived classes override this instead of NavigatedTo
    }

    public virtual void PostNavigatedTo(NavigationEventArgs e)
    {
        // Default no-op; derived can override for after-nav logic
    }

    public void OnNavigatedTo(NavigationEventArgs e)
    {
        InitializeAsync(e)
            .ContinueWith(x =>
            {
                NavigatedTo(e); // allow derived class to run custom logic

                if (Regions is not null)
                {
                    // Base class region handling (if needed)
                }

                PostNavigatedTo(e); // call post-navigation logic last
            });
    }

    public virtual void OnNavigatedFrom(NavigationEventArgs e) { }

    public virtual (string, string)? Regions => null;
}
