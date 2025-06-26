using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.Navigation;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Common;

public partial class BaseViewModel(BaseServices Services)
    : BaseBaseViewModel<RoutedEventArgs>(Services) { }
