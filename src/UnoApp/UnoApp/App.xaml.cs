using Uno.Resizetizer;
using Shiny.Extensions.DependencyInjection;
using UnoApp.Presentation.Pages.Main;
using UnoApp.Startup;
using WixApi.Models;
using SharedModels.Dtos.Contacts;

namespace UnoApp;

public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host =>
                host.AddEnvironment()
                    .AddLogging()
                    // Enable localization (see appsettings.json for supported languages)
                    .AddLocalization()
                    // Register Json serializers (ISerializer and ISerializer)
                    .AddSerialization()
                    .AddHttp()
                    .ConfigureServices(
                        (context, services) =>
                        {
                            services.AddCommonServices(context.Configuration);
                        }
                    )
                    .UseNavigation(RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<MainPage, MainViewModel>()
        );

        routes.Register(
            new RouteMap(
                "",
                View: views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new(
                        "Main",
                        View: views.FindByViewModel<MainViewModel>(),
                        IsDefault: true
                    ),
                ]
            )
        );
    }
}
