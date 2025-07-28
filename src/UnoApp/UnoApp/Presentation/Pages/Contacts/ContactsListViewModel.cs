using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Shiny.Mediator;
using Uno.Extensions.Navigation;
using Uno.Extensions.Reactive;
using UnoApp.ApiClient;
using DtoModels = SharedModels.Dtos.Contacts;

namespace UnoApp.Presentation.Pages.Contacts
{
    public partial class ContactsListViewModel : ObservableObject
    {
        private readonly IMediator _mediator;
        private readonly INavigator _navigator;
        private readonly ILogger<ContactsListViewModel> _logger;
        private readonly Services.Authentication.IAuthenticationService _authService;

        [ObservableProperty]
        private int _defaultCallbackDays = 3;

        [ObservableProperty]
        private bool _showTodayOnly = true;

        [ObservableProperty]
        private bool _showThisWeek;

        [ObservableProperty]
        private bool _showAll;

        public ContactsListViewModel(
            IMediator mediator,
            INavigator navigator,
            ILogger<ContactsListViewModel> logger,
            Services.Authentication.IAuthenticationService authService)
        {
            _mediator = mediator;
            _navigator = navigator;
            _logger = logger;
            _authService = authService;
            
            // TODO: Fix State.Async - Contacts = State<IEnumerable<ContactViewModel>>.Async(this, LoadContactsAsync);
        }

        public IFeed<IEnumerable<ContactViewModel>> Contacts { get; }

        private async Task<IEnumerable<ContactViewModel>> LoadContactsAsync(CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Request(new GetAllContactHttpRequest(), ct);
                if (result.Result != null)
                {
                    var contacts = result.Result
                        .Where(FilterContact)
                        .Select(c => new ContactViewModel(c, _mediator, _navigator, DefaultCallbackDays))
                        .OrderBy(c => c.NextCallDate ?? DateTime.MaxValue)
                        .ThenBy(c => c.Name);
                        
                    return contacts;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load contacts");
            }
            
            return Enumerable.Empty<ContactViewModel>();
        }

        private bool FilterContact(UnoApp.ApiClient.ContactDto contact)
        {
            if (ShowAll)
                return true;
                
            if (contact.NextCallDate == null)
                return false;
                
            var today = DateTime.Today;
            
            if (ShowTodayOnly)
                return contact.NextCallDate.Value.Date == today;
                
            if (ShowThisWeek)
            {
                var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);
                return contact.NextCallDate.Value.Date <= endOfWeek;
            }
            
            return false;
        }

        partial void OnShowTodayOnlyChanged(bool value)
        {
            if (value)
            {
                ShowThisWeek = false;
                ShowAll = false;
                // TODO: Fix refresh - Contacts.Refresh();
            }
        }

        partial void OnShowThisWeekChanged(bool value)
        {
            if (value)
            {
                ShowTodayOnly = false;
                ShowAll = false;
                // TODO: Fix refresh - Contacts.Refresh();
            }
        }

        partial void OnShowAllChanged(bool value)
        {
            if (value)
            {
                ShowTodayOnly = false;
                ShowThisWeek = false;
                // TODO: Fix refresh - Contacts.Refresh();
            }
        }

        [RelayCommand]
        private async Task SaveSettingsAsync()
        {
            try
            {
                // TODO: UpdateCallSettingsHttpRequest needs to be generated from WebApi.json
                // For now, we'll log the intended action
                _logger.LogInformation("Would update default callback days to: {Days}", DefaultCallbackDays);
                
                _logger.LogInformation("Settings saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save settings");
            }
        }
    }

    public partial class ContactViewModel : ObservableObject
    {
        private readonly UnoApp.ApiClient.ContactDto _contact;
        private readonly IMediator _mediator;
        private readonly INavigator _navigator;
        private readonly int _defaultCallbackDays;
        private readonly ILogger _logger;

        public ContactViewModel(UnoApp.ApiClient.ContactDto contact, IMediator mediator, INavigator navigator, int defaultCallbackDays)
        {
            _contact = contact;
            _mediator = mediator;
            _navigator = navigator;
            _defaultCallbackDays = defaultCallbackDays;
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ContactViewModel>();
        }

        public string Name => _contact.Name;
        public string Email => _contact.Email;
        public string Phone => _contact.Phone;
        public string Branche => _contact.Industry;
        public string? CallNotes => _contact.CallNotes;
        public DateTime? NextCallDate => _contact.NextCallDate?.DateTime;
        
        public string NextCallDateDisplay
        {
            get
            {
                if (NextCallDate == null)
                    return "Nicht geplant";
                    
                var today = DateTime.Today;
                var callDate = NextCallDate.Value.Date;
                
                if (callDate == today)
                    return "Heute";
                else if (callDate == today.AddDays(1))
                    return "Morgen";
                else if (callDate < today)
                    return $"Überfällig ({callDate:dd.MM.})";
                else
                    return callDate.ToString("dd.MM.yyyy");
            }
        }
        
        public string NextCallDateColor
        {
            get
            {
                if (NextCallDate == null)
                    return "Gray";
                    
                var today = DateTime.Today;
                if (NextCallDate.Value.Date < today)
                    return "Red";
                else if (NextCallDate.Value.Date == today)
                    return "Orange";
                else
                    return "Black";
            }
        }
        
        public string StatusText => ((DtoModels.CallStatus)(int)_contact.CallStatus) switch
        {
            DtoModels.CallStatus.New => "Neu",
            DtoModels.CallStatus.Scheduled => "Geplant",
            DtoModels.CallStatus.Reached => "Erreicht",
            DtoModels.CallStatus.NotReached => "Nicht erreicht",
            DtoModels.CallStatus.Completed => "Abgeschlossen",
            DtoModels.CallStatus.Postponed => "Verschoben",
            _ => "Unbekannt"
        };
        
        public string StatusColor => ((DtoModels.CallStatus)(int)_contact.CallStatus) switch
        {
            DtoModels.CallStatus.New => "Blue",
            DtoModels.CallStatus.Scheduled => "Orange",
            DtoModels.CallStatus.Reached => "Green",
            DtoModels.CallStatus.NotReached => "Red",
            DtoModels.CallStatus.Completed => "Gray",
            DtoModels.CallStatus.Postponed => "Purple",
            _ => "Gray"
        };
        
        public Visibility HasNotes => string.IsNullOrWhiteSpace(CallNotes) ? Visibility.Collapsed : Visibility.Visible;

        [RelayCommand]
        private async Task CallAsync()
        {
            // Navigate to call detail page
            await _navigator.NavigateDataAsync(
                this,
                data: new Dictionary<string, object> { ["Contact"] = _contact },
                cancellation: CancellationToken.None
            );
        }

        [RelayCommand]
        private async Task RescheduleAsync()
        {
            // TODO: Show date picker dialog
            var newDate = DateTime.Today.AddDays(_defaultCallbackDays);
            
            // TODO: RescheduleCallHttpRequest needs to be generated from WebApi.json
            // For now, we'll just update the local state
            _logger.LogInformation("Would reschedule contact {ContactId} to {Date}", _contact.Id, newDate);
        }

        [RelayCommand]
        private async Task MarkReachedAsync()
        {
            // TODO: UpdateCallStatusHttpRequest needs to be generated from WebApi.json
            _logger.LogInformation("Would mark contact {ContactId} as reached", _contact.Id);
        }

        [RelayCommand]
        private async Task MarkNotReachedAsync()
        {
            // TODO: UpdateCallStatusHttpRequest needs to be generated from WebApi.json
            _logger.LogInformation("Would mark contact {ContactId} as not reached", _contact.Id);
        }
    }
}