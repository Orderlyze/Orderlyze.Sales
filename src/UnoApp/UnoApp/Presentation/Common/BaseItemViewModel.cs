using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Common;

internal class BaseItemViewModel<TItem>(BaseServices BaseServices, TItem Item)
    : BaseViewModel(BaseServices)
    where TItem : class;
