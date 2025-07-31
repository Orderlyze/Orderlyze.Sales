using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Presentation.Pages.WixContacts;
using UnoApp.Services.Common;
using Shiny.Mediator;
using Microsoft.UI.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using Uno.Extensions.Reactive;

namespace UnoApp.Presentation.Views.WixContacts;

internal partial class WixContactsListViewModel
    : BaseItemViewModel<IEnumerable<WixContactsListModel>>
{
    private readonly IMediator _mediator;
    public IState<DateTimeOffset> SelectedDate { get; }

    public WixContactsListViewModel(
        BaseServices services,
        IFeed<IEnumerable<WixContactsListModel>> wixContacts,
        IMediator mediator
    )
        : base(services, wixContacts) 
    { 
        _mediator = mediator;
        SelectedDate = State.Value(this, () => DateTimeOffset.Now);
        
        // Watch for date changes
        SelectedDate.ForEachAsync(async (date, ct) =>
        {
            if (PageViewModel != null)
            {
                PageViewModel.OnDateChanged(date);
            }
        });
    }

    public WixContactsPageViewModel? PageViewModel { get; set; }

    public override Task InitializeAsync(RoutedEventArgs e)
    {
        return base.InitializeAsync(e);
    }

    public async Task AddContactAsync(WixContactsListModel wixContact)
    {
        try
        {
            // TODO: AddContactHttpRequest is not being generated from WebApi.json
            // The /Contact/add endpoint needs to be properly exposed in the WebApi
            // For now, this functionality is disabled
            
            /*
            var request = new AddContactHttpRequest
            {
                Body = new AddContactRequest
                {
                    WixId = wixContact.Id ?? string.Empty,
                    Name = wixContact.Name ?? string.Empty,
                    Email = wixContact.Email ?? string.Empty,
                    Phone = wixContact.Phone ?? string.Empty,
                    Industry = wixContact.Branche ?? string.Empty
                }
            };

            var result = await _mediator.Request(request);
            */
            
            // Show message
            await ShowMessageAsync($"Kontakt hinzufügen ist momentan deaktiviert - AddContactHttpRequest fehlt");
        }
        catch (Exception ex)
        {
            // Show error message
            await ShowMessageAsync($"Fehler beim Hinzufügen des Kontakts: {ex.Message}");
        }
    }

    private async Task ShowMessageAsync(string message)
    {
        // TODO: Implement proper notification system
        // For now, just log the message
        Console.WriteLine(message);
        await Task.CompletedTask;
    }
}
