using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Common.ViewModels;

internal class BaseItemViewModel<TItem> : BaseViewModel
    where TItem : class
{
    public IFeed<TItem> Item { get; set; }

    public BaseItemViewModel(BaseServices baseServices, IFeed<TItem> item)
        : base(baseServices)
    {
        this.Item = item;
    }
}
