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
    private readonly IState<DateTimeOffset> _selectedDate;
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
        
        // Initialize the state with the current date
        _selectedDate = State.Value(this, () => DateTimeOffset.Now);
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
        // Update the state value
        _ = _selectedDate.UpdateAsync(_ => newDate);
    }

    public IFeed<IEnumerable<WixContactsListModel>> WixContacts =>
        _selectedDate.SelectAsync(async (selectedDate, ct) =>
        {
            try
            {
                var response = await _mediator.Request(new ContactsHttpRequest(), ct);
                
                if (response.Result != null)
                {
                    var allContacts = response.Result
                        .Where(x => x.CreatedDate.HasValue && 
                                   x.CreatedDate.Value.Date == selectedDate.Date)
                        .Select(x => new WixContactsListModel(
                            x.Id,
                            $"{x.FirstName} {x.LastName}".Trim(),
                            x.Email,
                            x.Phone,
                            x.Address, // Branche - using address as branch
                            x.Company,
                            x.LabelKeys?.ToArray() ?? Array.Empty<string>()
                        ));

                    return allContacts;
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