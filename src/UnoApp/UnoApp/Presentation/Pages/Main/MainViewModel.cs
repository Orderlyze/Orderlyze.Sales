using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Presentation.Pages.Main;

public partial class MainViewModel : ObservableObject
{
    private INavigator _navigator;

    [ObservableProperty]
    private string? name;

    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator
    )
    {
        _navigator = navigator;
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
        GoToSecond = new AsyncRelayCommand(GoToSecondView);
        Wix();
    }

    public string? Title { get; }

    public ICommand GoToSecond { get; }

    public List<Entity> Entities { get; set; } = new List<Entity> { new Entity("Entity 1") };

    private async Task GoToSecondView()
    {
        await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Name!));
    }

    private async Task Wix()
    {
        await _navigator.NavigateRouteAsync(this, "./WixRegion/WixContacts");
    }
}
