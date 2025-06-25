using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Presentation.Common;

internal partial class BaseViewModel : ObservableObject
{
    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual void NavigatedTo()
    {
        if (Regions is not null) { }
    }

    public virtual (string, string)? Regions => null;
}
