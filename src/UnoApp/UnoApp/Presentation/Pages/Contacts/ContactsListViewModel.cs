using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SharedModels.Dtos.Contacts;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;
using Uno.Extensions.Navigation;
using Uno.Extensions.Reactive;
using UnoApp.ApiClient;
using UnoApp.Presentation.Common.ViewModels;

namespace UnoApp.Presentation.Pages.Contacts
{
    [SingletonService]
    public partial class ContactsListViewModel : BaseViewModel
    {
        private readonly IMediator _mediator;
        private readonly INavigator _navigator;
        private readonly ILogger<ContactsListViewModel> _logger;
        private readonly Authentication.IAuthenticationService _authService;

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
            Authentication.IAuthenticationService authService)
        {
            _mediator = mediator;
            _navigator = navigator;
            _logger = logger;
            _authService = authService;
            
            Contacts = State.Async(LoadContactsAsync);
        }

        public IFeed<IEnumerable<ContactViewModel>> Contacts { get; }

        private async Task<IEnumerable<ContactViewModel>> LoadContactsAsync(CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Request(new GetAllContactsHttpRequest(), ct);
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

        private bool FilterContact(ContactDto contact)
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
                _ = Contacts.Refresh();
            }
        }

        partial void OnShowThisWeekChanged(bool value)
        {
            if (value)
            {
                ShowTodayOnly = false;
                ShowAll = false;
                _ = Contacts.Refresh();
            }
        }

        partial void OnShowAllChanged(bool value)
        {
            if (value)
            {
                ShowTodayOnly = false;
                ShowThisWeek = false;
                _ = Contacts.Refresh();
            }
        }

        [RelayCommand]
        private async Task SaveSettingsAsync()
        {
            try
            {
                await _mediator.Send(new UpdateCallSettingsHttpRequest
                {
                    Body = new UpdateCallSettingsRequest
                    {
                        DefaultCallbackDays = DefaultCallbackDays
                    }
                });
                
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
        private readonly ContactDto _contact;
        private readonly IMediator _mediator;
        private readonly INavigator _navigator;
        private readonly int _defaultCallbackDays;

        public ContactViewModel(ContactDto contact, IMediator mediator, INavigator navigator, int defaultCallbackDays)
        {
            _contact = contact;
            _mediator = mediator;
            _navigator = navigator;
            _defaultCallbackDays = defaultCallbackDays;
        }

        public string Name => _contact.Name;
        public string Email => _contact.Email;
        public string Phone => _contact.Phone;
        public string Branche => _contact.Branche;
        public string? CallNotes => _contact.CallNotes;
        public DateTime? NextCallDate => _contact.NextCallDate;
        
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
        
        public string StatusText => _contact.CallStatus switch
        {
            CallStatus.New => "Neu",
            CallStatus.Scheduled => "Geplant",
            CallStatus.Reached => "Erreicht",
            CallStatus.NotReached => "Nicht erreicht",
            CallStatus.Completed => "Abgeschlossen",
            CallStatus.Postponed => "Verschoben",
            _ => "Unbekannt"
        };
        
        public string StatusColor => _contact.CallStatus switch
        {
            CallStatus.New => "Blue",
            CallStatus.Scheduled => "Orange",
            CallStatus.Reached => "Green",
            CallStatus.NotReached => "Red",
            CallStatus.Completed => "Gray",
            CallStatus.Postponed => "Purple",
            _ => "Gray"
        };
        
        public Visibility HasNotes => string.IsNullOrWhiteSpace(CallNotes) ? Visibility.Collapsed : Visibility.Visible;

        [RelayCommand]
        private async Task CallAsync()
        {
            // Navigate to call detail page
            await _navigator.NavigateDataAsync(
                new Dictionary<string, object> { ["Contact"] = _contact }
            );
        }

        [RelayCommand]
        private async Task RescheduleAsync()
        {
            // TODO: Show date picker dialog
            var newDate = DateTime.Today.AddDays(_defaultCallbackDays);
            
            await _mediator.Request(new RescheduleCallHttpRequest
            {
                Body = new RescheduleCallRequest
                {
                    ContactId = _contact.Id.GetHashCode(),
                    NewCallDate = newDate
                }
            });
        }

        [RelayCommand]
        private async Task MarkReachedAsync()
        {
            await _mediator.Request(new UpdateCallStatusHttpRequest
            {
                Body = new UpdateCallStatusRequest
                {
                    ContactId = _contact.Id.GetHashCode(),
                    Status = CallStatus.Reached,
                    Notes = "Erfolgreich erreicht"
                }
            });
        }

        [RelayCommand]
        private async Task MarkNotReachedAsync()
        {
            await _mediator.Request(new UpdateCallStatusHttpRequest
            {
                Body = new UpdateCallStatusRequest
                {
                    ContactId = _contact.Id.GetHashCode(),
                    Status = CallStatus.NotReached,
                    Notes = "Nicht erreicht",
                    RescheduleCall = true
                }
            });
        }
    }
}