using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace UnoApp.Presentation.Views.WixContacts;

public partial class WixContactsListView
{
    public WixContactsListView()
    {
        this.InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is WixContactsListViewModel viewModel)
        {
            // Set initial date
            ContactDatePicker.Date = viewModel.SelectedDate;
        }
    }

    private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is WixContactsListModel contact && DataContext is WixContactsListViewModel viewModel)
        {
            // Import the contact using the page's import command
            if (viewModel.PageViewModel?.ImportContactCommand?.CanExecute(contact.Id) == true)
            {
                await viewModel.PageViewModel.ImportContactCommand.ExecuteAsync(contact.Id);
            }
        }
    }

    private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs args)
    {
        if (DataContext is WixContactsListViewModel viewModel)
        {
            // Update the selected date in the view model
            viewModel.SelectedDate = args.NewDate;
        }
    }
}