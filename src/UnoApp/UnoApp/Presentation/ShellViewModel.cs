namespace UnoApp.Presentation;

using UnoApp.Presentation.Pages.Main;

public class ShellViewModel
{
    private readonly INavigator _navigator;

    public ShellViewModel(
        INavigator navigator)
    {
        _navigator = navigator;
        Console.WriteLine("ShellViewModel constructor called");
        // Add code here to initialize or attach event handlers to singleton services
        
        // Navigate to Main page after Shell loads
        _ = Task.Run(async () => await NavigateToMainAsync());
    }
    
    private async Task NavigateToMainAsync()
    {
        try
        {
            Console.WriteLine("NavigateToMainAsync called");
            // Small delay to ensure Shell is fully loaded
            await Task.Delay(500);
            Console.WriteLine("Navigating to MainViewModel");
            var result = await _navigator.NavigateViewModelAsync<MainViewModel>(this);
            Console.WriteLine($"Navigation result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Navigation error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
