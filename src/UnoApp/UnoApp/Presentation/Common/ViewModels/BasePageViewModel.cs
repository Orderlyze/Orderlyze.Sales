using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Common;

public partial class BasePageViewModel(BaseServices Services)
    : BaseBaseViewModel<NavigationEventArgs>(Services);
