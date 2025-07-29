using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using UnoApp.Navigation;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Pages.Main;
using UnoApp.Presentation.Pages.Register;
using UnoApp.Services.Common;
using Authentication = UnoApp.Services.Authentication;
using ICommand = System.Windows.Input.ICommand;

namespace UnoApp.Presentation.Pages.Login;

internal partial class LoginPageViewModel : BasePageViewModel
{
    private readonly Authentication.IAuthenticationService _authService;
    private readonly INavigator _navigator;
    private readonly ILogger<LoginPageViewModel> _logger;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    public LoginPageViewModel(
        BaseServices baseServices,
        Authentication.IAuthenticationService authService,
        INavigator navigator,
        ILogger<LoginPageViewModel> logger) : base(baseServices)
    {
        _authService = authService;
        _navigator = navigator;
        _logger = logger;
        
        LoginCommand = new AsyncRelayCommand(LoginAsync);
        NavigateToRegisterCommand = new AsyncRelayCommand(NavigateToRegisterAsync);
    }

    public ICommand LoginCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }

    private async Task LoginAsync()
    {
        _logger.LogInformation("LoginAsync called with Email: {Email}", Email);
        
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter email and password";
            _logger.LogWarning("Login attempt with empty credentials");
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = null;
            
            _logger.LogInformation("Calling authentication service for {Email}", Email);
            var success = await _authService.LoginAsync(Email, Password);
            
            _logger.LogInformation("Authentication service returned: {Success}", success);
            
            if (success)
            {
                _logger.LogInformation("Login successful for {Email}, navigating to main page", Email);
                // Navigate to main page after successful login
                await _navigator.NavigateViewModelAsync<MainViewModel>(this);
            }
            else
            {
                ErrorMessage = "Invalid email or password";
                _logger.LogWarning("Login failed for {Email} - authentication service returned false", Email);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error in ViewModel");
            ErrorMessage = "An error occurred during login. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private async Task NavigateToRegisterAsync()
    {
        await _navigator.NavigateViewModelAsync<RegisterPageViewModel>(this);
    }
}