using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Shiny.Mediator;
using UnoApp.ApiClient;
using UnoApp.Presentation.Pages.Login;
using UnoApp.Presentation.Pages.Main;
using Authentication = UnoApp.Services.Authentication;

namespace UnoApp.Presentation.Pages.Register;

public partial class RegisterPageViewModel : ObservableObject
{
    private readonly IMediator _mediator;
    private readonly Authentication.IAuthenticationService _authService;
    private readonly INavigator _navigator;
    private readonly ILogger<RegisterPageViewModel> _logger;

    [ObservableProperty]
    private string _email = "";

    [ObservableProperty]
    private string _password = "";

    [ObservableProperty]
    private string _confirmPassword = "";

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty]
    private bool _hasError;

    public bool CanRegister => !string.IsNullOrWhiteSpace(Email) && 
                               !string.IsNullOrWhiteSpace(Password) && 
                               !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                               !IsLoading;

    public RegisterPageViewModel(
        IMediator mediator,
        Authentication.IAuthenticationService authService,
        INavigator navigator,
        ILogger<RegisterPageViewModel> logger)
    {
        _mediator = mediator;
        _authService = authService;
        _navigator = navigator;
        _logger = logger;
    }

    partial void OnEmailChanged(string value) => OnPropertyChanged(nameof(CanRegister));
    partial void OnPasswordChanged(string value) => OnPropertyChanged(nameof(CanRegister));
    partial void OnConfirmPasswordChanged(string value) => OnPropertyChanged(nameof(CanRegister));

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = "";
        HasError = false;

        // Validate inputs
        if (!IsValidEmail(Email))
        {
            ErrorMessage = "Bitte geben Sie eine gültige E-Mail-Adresse ein.";
            HasError = true;
            return;
        }

        if (Password.Length < 6)
        {
            ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.";
            HasError = true;
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Die Passwörter stimmen nicht überein.";
            HasError = true;
            return;
        }

        IsLoading = true;

        try
        {
            var registerRequest = new RegisterHttpRequest
            {
                Body = new RegisterRequest
                {
                    Email = Email,
                    Password = Password
                }
            };

            var result = await _mediator.Request(registerRequest);
            
            if (result.Result?.IsSuccessStatusCode == true)
            {
                _logger.LogInformation("Registration successful for {Email}", Email);
                
                // Nach erfolgreicher Registrierung automatisch einloggen
                var loginSuccess = await _authService.LoginAsync(Email, Password);
                
                if (loginSuccess)
                {
                    await _navigator.NavigateViewModelAsync<MainViewModel>(this);
                }
                else
                {
                    // Falls Login fehlschlägt, zur Login-Seite navigieren
                    await _navigator.NavigateViewModelAsync<LoginPageViewModel>(this);
                }
            }
            else
            {
                var errorContent = result.Result?.Content != null 
                    ? await result.Result.Content.ReadAsStringAsync() 
                    : "Unbekannter Fehler";
                
                ErrorMessage = $"Registrierung fehlgeschlagen: {errorContent}";
                HasError = true;
                _logger.LogWarning("Registration failed: {Error}", errorContent);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ein Fehler ist aufgetreten. Bitte versuchen Sie es später erneut.";
            HasError = true;
            _logger.LogError(ex, "Registration error");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToLoginAsync()
    {
        await _navigator.NavigateBackAsync(this);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
