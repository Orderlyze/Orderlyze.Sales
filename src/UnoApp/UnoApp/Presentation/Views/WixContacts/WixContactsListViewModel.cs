using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoApp.ApiClient;
using UnoApp.Presentation.Common.ViewModels;
using UnoApp.Services.Common;
using WixApi.Models;

namespace UnoApp.Presentation.Views.WixContacts;

internal partial class WixContactsListViewModel
    : BaseItemViewModel<IEnumerable<WixContactsListModel>>
{
    private readonly IMediator _mediator;

    public WixContactsListViewModel(
        BaseServices services,
        IFeed<IEnumerable<WixContactsListModel>> wixContacts,
        IMediator mediator
    )
        : base(services, wixContacts) 
    { 
        _mediator = mediator;
    }

    public override Task InitializeAsync(RoutedEventArgs e)
    {
        return base.InitializeAsync(e);
    }

    public async Task AddContactAsync(WixContactsListModel wixContact)
    {
        try
        {
            var request = new AddContactHttpRequest
            {
                Body = new AddContactRequest
                {
                    WixId = wixContact.Id ?? string.Empty,
                    Name = wixContact.Name ?? string.Empty,
                    Email = wixContact.Email ?? string.Empty,
                    Phone = wixContact.Phone ?? string.Empty,
                    Branche = wixContact.Branche ?? string.Empty
                }
            };

            var result = await _mediator.Request(request);
            
            // Show success message
            await ShowMessageAsync($"Kontakt '{wixContact.Name}' wurde erfolgreich hinzugefügt!");
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
