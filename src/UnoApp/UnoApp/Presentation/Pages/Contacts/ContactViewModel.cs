using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Services.Common;
using DtoModels = SharedModels.Dtos.Contacts;

namespace UnoApp.Presentation.Pages.Contacts
{
    public partial class ContactViewModel : BaseItemViewModel<ContactDto>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContactViewModel> _logger;
        private readonly int _defaultCallbackDays;

        public ContactViewModel(
            BaseServices services,
            IMediator mediator,
            ILogger<ContactViewModel> logger,
            ContactDto contact,
            int defaultCallbackDays = 7)
            : base(services, Feed.Single(contact))
        {
            _mediator = mediator;
            _logger = logger;
            _defaultCallbackDays = defaultCallbackDays;
        }

        // Reactive properties derived from the contact feed
        public IFeed<string> Name => Item.Select(c => c?.Name ?? string.Empty);
        public IFeed<string> Email => Item.Select(c => c?.Email ?? string.Empty);
        public IFeed<string> Phone => Item.Select(c => c?.Phone ?? string.Empty);
        public IFeed<string> Branche => Item.Select(c => c?.Industry ?? string.Empty);
        public IFeed<string?> CallNotes => Item.Select(c => c?.CallNotes);
        public IFeed<DateTime?> NextCallDate => Item.Select(c => c?.NextCallDate?.DateTime);
        
        public IFeed<string> NextCallDateDisplay => NextCallDate.Select(date =>
        {
            if (date == null)
                return "Nicht geplant";
                
            var today = DateTime.Today;
            var callDate = date.Value.Date;
            
            if (callDate == today)
                return "Heute";
            else if (callDate == today.AddDays(1))
                return "Morgen";
            else if (callDate < today)
                return $"Überfällig ({callDate:dd.MM.})";
            else
                return callDate.ToString("dd.MM.yyyy");
        });
        
        public IFeed<string> NextCallDateColor => NextCallDate.Select(date =>
        {
            if (date == null)
                return "Gray";
                
            var today = DateTime.Today;
            if (date.Value.Date < today)
                return "Red";
            else if (date.Value.Date == today)
                return "Orange";
            else
                return "Black";
        });
        
        public IFeed<string> StatusText => Item.Select(contact => 
            contact == null ? "Unbekannt" : ((DtoModels.CallStatus)(int)contact.CallStatus) switch
            {
                DtoModels.CallStatus.New => "Neu",
                DtoModels.CallStatus.Scheduled => "Geplant",
                DtoModels.CallStatus.Reached => "Erreicht",
                DtoModels.CallStatus.NotReached => "Nicht erreicht",
                DtoModels.CallStatus.Completed => "Abgeschlossen",
                DtoModels.CallStatus.Postponed => "Verschoben",
                _ => "Unbekannt"
            });
        
        public IFeed<string> StatusColor => Item.Select(contact => 
            contact == null ? "Gray" : ((DtoModels.CallStatus)(int)contact.CallStatus) switch
            {
                DtoModels.CallStatus.New => "Blue",
                DtoModels.CallStatus.Scheduled => "Orange",
                DtoModels.CallStatus.Reached => "Green",
                DtoModels.CallStatus.NotReached => "Red",
                DtoModels.CallStatus.Completed => "Gray",
                DtoModels.CallStatus.Postponed => "Purple",
                _ => "Gray"
            });
        
        public IFeed<Visibility> HasNotes => CallNotes.Select(notes => 
            string.IsNullOrWhiteSpace(notes) ? Visibility.Collapsed : Visibility.Visible);

        [RelayCommand]
        private async Task CallAsync(CancellationToken cancellationToken)
        {
            var contact = await Item.FirstAsync(cancellationToken);
            if (contact == null) return;
            
            // Navigate to call detail page
            await Services.Navigator.NavigateDataAsync(
                this,
                data: new Dictionary<string, object> { ["Contact"] = contact },
                cancellation: cancellationToken
            );
        }

        [RelayCommand]
        private async Task RescheduleAsync(CancellationToken cancellationToken)
        {
            var contact = await Item.FirstAsync(cancellationToken);
            if (contact == null) return;
            
            // TODO: Show date picker dialog
            var newDate = DateTime.Today.AddDays(_defaultCallbackDays);
            
            // TODO: RescheduleCallHttpRequest needs to be generated from WebApi.json
            // For now, we'll just update the local state
            _logger.LogInformation("Would reschedule contact {ContactId} to {Date}", contact.Id, newDate);
        }

        [RelayCommand]
        private async Task MarkReachedAsync(CancellationToken cancellationToken)
        {
            var contact = await Item.FirstAsync(cancellationToken);
            if (contact == null) return;
            
            // TODO: UpdateCallStatusHttpRequest needs to be generated from WebApi.json
            _logger.LogInformation("Would mark contact {ContactId} as reached", contact.Id);
        }

        [RelayCommand]
        private async Task MarkNotReachedAsync(CancellationToken cancellationToken)
        {
            var contact = await Item.FirstAsync(cancellationToken);
            if (contact == null) return;
            
            // TODO: UpdateCallStatusHttpRequest needs to be generated from WebApi.json
            _logger.LogInformation("Would mark contact {ContactId} as not reached", contact.Id);
        }
    }
}