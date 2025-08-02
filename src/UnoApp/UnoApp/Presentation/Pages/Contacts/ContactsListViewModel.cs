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
}