using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using UnoApp.Navigation;
using UnoApp.Presentation.Common;
using UnoApp.Services.Common;

namespace UnoApp.Presentation.Pages.Main;

public partial class MainViewModel : BasePageViewModel
{
    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator,
        BaseServices baseServices
    )
        : base(baseServices) { }

    //public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
    //{
    //    return [new RegionModel("WixRegion", "WixContacts")];
    //}
}
