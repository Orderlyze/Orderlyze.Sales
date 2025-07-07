namespace UnoApp.Presentation;

using UnoApp.Presentation.Pages.Main;

public class ShellViewModel
{
    private readonly INavigator _navigator;

    public ShellViewModel(
        INavigator navigator)
    {
        _navigator = navigator;
        // Add code here to initialize or attach event handlers to singleton services
        
        // Navigate to Main page after Shell loads
        _ = NavigateToMainAsync();
    }
    
    private async Task NavigateToMainAsync()
    {
        // Small delay to ensure Shell is fully loaded
        await Task.Delay(100);
        await _navigator.NavigateViewModelAsync<MainViewModel>(this);
    }
}
