using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Shiny.Mediator;
using UnoApp.ApiClient;
using UnoApp.Constants;
using UnoApp.Navigation;
using UnoApp.Presentation.Common;
using UnoApp.Presentation.Views.WixContacts;
using UnoApp.Services.Common;
using Microsoft.UI.Dispatching;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Uno.Extensions.Reactive;

namespace UnoApp.Presentation.Pages.WixContacts;

internal partial class WixContactsPageViewModel : BasePageViewModel
{
    private readonly INavigator _navigator;
    private readonly IMediator _mediator;
    private readonly BaseServices _baseServices;
    private DateTimeOffset _selectedDate = DateTimeOffset.Now;
    private WixContactsListViewModel? _listViewModel;

    public WixContactsPageViewModel(
        BaseServices baseServices,
        INavigator navigator,
        IMediator mediator
    )
        : base(baseServices)
    {
        _navigator = navigator;
        _mediator = mediator;
        _baseServices = baseServices;
        ImportContactCommand = new AsyncRelayCommand<string>(ImportContactAsync);
        
        // Initialize with current date
        _selectedDate = DateTimeOffset.Now;
    }

    public IAsyncRelayCommand<string> ImportContactCommand { get; }

    public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
    {
        _listViewModel = new WixContactsListViewModel(
            _baseServices,
            WixContacts,
            _mediator
        )
        {
            PageViewModel = this
        };
        
        return [new("List", RegionViewsNames.WixContactList, data: _listViewModel)];
    }

    public void OnDateChanged(DateTimeOffset newDate)
    {
        // Update the selected date
        _selectedDate = newDate;
        Console.WriteLine($"[PageViewModel] Date changed to: {newDate:yyyy-MM-dd}");
    }

    public IFeed<IEnumerable<WixContactsListModel>> WixContacts =>
        Feed.Async(async ct =>
        {
            try
            {
                Console.WriteLine($"[WixContacts] Fetching contacts for date: {_selectedDate:yyyy-MM-dd}");
                var response = await _mediator.Request(new ContactsHttpRequest(), ct);
                
                if (response.Result != null)
                {
                    Console.WriteLine($"[WixContacts] Total contacts received: {response.Result.Count()}");
                    
                    // Log some sample dates
                    var sampleContacts = response.Result.Take(5);
                    foreach (var contact in sampleContacts)
                    {
                        Console.WriteLine($"[WixContacts] Contact: {contact.FirstName} {contact.LastName}, Created: {contact.CreatedDate:yyyy-MM-dd HH:mm:ss}");
                    }
                    
                    var filteredContacts = response.Result
                        .Where(x => x.CreatedDate.HasValue && 
                                   x.CreatedDate.Value.Date == _selectedDate.Date)
                        .ToList();
                        
                    Console.WriteLine($"[WixContacts] Filtered contacts for {_selectedDate:yyyy-MM-dd}: {filteredContacts.Count}");
                    
                    var result = filteredContacts.Select(x => new WixContactsListModel(
                        x.Id,
                        $"{x.FirstName} {x.LastName}".Trim(),
                        x.Email,
                        x.Phone,
                        x.Address, // Branche - using address as branch
                        x.Company,
                        x.LabelKeys?.ToArray() ?? Array.Empty<string>()
                    ));

                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                Console.WriteLine($"Error fetching Wix contacts: {ex.Message}");
            }
            
            return Enumerable.Empty<WixContactsListModel>();
        });

    private async Task ImportContactAsync(string? wixContactId)
    {
        if (string.IsNullOrEmpty(wixContactId))
            return;

        try
        {
            // Show loading state if needed
            await _mediator.Request(new ImportWixHttpRequest
            {
                Body = new ImportWixContactRequest { WixContactId = wixContactId }
            });

            // Show success message
            // TODO: Add user feedback for successful import
            
            // Optionally navigate to contacts page
            // await _navigator.NavigateAsync(Routes.Contacts);
        }
        catch (Exception ex)
        {
            // Handle error - show user feedback
            Console.WriteLine($"Error importing contact: {ex.Message}");
            // TODO: Add user error feedback
        }
    }
}