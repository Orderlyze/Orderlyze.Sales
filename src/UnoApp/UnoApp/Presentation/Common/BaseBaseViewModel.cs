using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Navigation;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Common;

public abstract class BaseBaseViewModel<TNavigationEventArgs>(BaseServices Services)
    : ObservableObject,
        INavigationAware<TNavigationEventArgs>
{
    public virtual Task InitializeAsync(TNavigationEventArgs e)
    {
        return Task.CompletedTask;
    }

    protected virtual void NavigatedTo(TNavigationEventArgs e)
    {
        // Default no-op; derived classes override this instead of NavigatedTo
    }

    public virtual void PostNavigatedTo(TNavigationEventArgs e)
    {
        // Default no-op; derived can override for after-nav logic
    }

    public void OnNavigatedTo(TNavigationEventArgs e)
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

    public virtual void OnNavigatedFrom(TNavigationEventArgs e) { }

    public virtual (string, string)? Regions => null;
}
