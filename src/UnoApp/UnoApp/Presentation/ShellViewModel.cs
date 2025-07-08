namespace UnoApp.Presentation;

using UnoApp.Presentation.Pages.Main;
using UnoApp.Presentation.Pages.Login;
using UnoApp.Services.Authentication;

public class ShellViewModel
{
    private readonly INavigator _navigator;
    private readonly IAuthenticationService _authService;

    public ShellViewModel(
        INavigator navigator,
        IAuthenticationService authService)
    {
        _navigator = navigator;
        _authService = authService;
        Console.WriteLine("ShellViewModel constructor called");
        // Add code here to initialize or attach event handlers to singleton services
        
        // Check authentication status and navigate accordingly
        _ = Task.Run(async () => await InitialNavigationAsync());
    }
    
    private async Task InitialNavigationAsync()
    {
        try
        {
            Console.WriteLine("InitialNavigationAsync called");
            // Small delay to ensure Shell is fully loaded
            await Task.Delay(500);
            
            // Check if user is authenticated
            if (_authService.IsAuthenticated)
            {
                Console.WriteLine("User is authenticated, navigating to MainViewModel");
                var result = await _navigator.NavigateViewModelAsync<MainViewModel>(this);
                Console.WriteLine($"Navigation to Main result: {result}");
            }
            else
            {
                Console.WriteLine("User is not authenticated, navigating to LoginPageViewModel");
                var result = await _navigator.NavigateViewModelAsync<LoginPageViewModel>(this);
                Console.WriteLine($"Navigation to Login result: {result}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
