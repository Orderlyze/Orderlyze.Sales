# MVVM Patterns for Orderlyze.Sales

## Overview
This document outlines the MVVM (Model-View-ViewModel) patterns used in the Orderlyze.Sales application, with special focus on integration with Uno.Reactive.

## Import References
@docs/reactive/FeedView.md
@docs/reactive/reactive-programming.md

## ViewModel Base Classes

### ViewModelBase
All ViewModels should inherit from UnoApp.Presentation.Common.ViewModels or appropriate specialized base classes.

### Specialized Base Classes
- `BasePageViewModel`: For page-level ViewModels
- `BaseItemViewModel`: For list item ViewModels

## ViewModel Structure

### 1. Property Declaration
```csharp
public partial class CustomerPageViewModel : BasePageViewModel
{
    // Services
    private readonly ICustomerService _customerService;
    private readonly INavigationService _navigationService;
    
    // Reactive States
    public IState<string> CustomerName => State<string>.Value(this, () => string.Empty);
    public IState<bool> IsLoading => State<bool>.Value(this, () => false);
    
    // Reactive Feeds
    public IFeed<bool> CanSave => CustomerName.Select(name => !string.IsNullOrWhiteSpace(name));
    
    // Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
}
```

### 2. Constructor Pattern
```csharp
public CustomerViewModel(
    ICustomerService customerService,
    INavigationService navigationService)
{
    _customerService = customerService;
    _navigationService = navigationService;
    
    // Initialize commands
    SaveCommand = new AsyncRelayCommand(SaveAsync, () => CanSave.Value);
    CancelCommand = new RelayCommand(Cancel);
    
    // Set up reactive subscriptions if needed
    CanSave.Subscribe(_ => SaveCommand.NotifyCanExecuteChanged())
           .DisposeWith(this);
}
```

## Command Patterns

### 1. Async Commands
```csharp
[RelayCommand]
private async Task LoadDataAsync(CancellationToken cancellationToken)
{
    await IsLoading.UpdateAsync(_ => true, cancellationToken);
    try
    {
        var data = await _service.GetDataAsync(cancellationToken);
        await ProcessDataAsync(data, cancellationToken);
    }
    finally
    {
        await IsLoading.UpdateAsync(_ => false, cancellationToken);
    }
}
```

### 2. Parameterized Commands
```csharp
[RelayCommand]
private async Task DeleteItemAsync(CustomerItem item, CancellationToken cancellationToken)
{
    await _service.DeleteAsync(item.Id, cancellationToken);
}
```

## Navigation Patterns

### 1. Navigation with Parameters
```csharp
private async Task NavigateToDetailsAsync(int customerId, CancellationToken ct)
{
    var parameters = new Dictionary<string, object>
    {
        ["customerId"] = customerId
    };
    
    await _navigationService.NavigateAsync<CustomerDetailViewModel>(parameters);
}
```

### 2. Navigation with Results
```csharp
private async Task OpenDialogAsync(CancellationToken ct)
{
    var result = await _navigationService.ShowDialogAsync<EditCustomerDialogViewModel, CustomerModel>(
        new CustomerModel { Name = CustomerName.Value });
    
    if (result != null)
    {
        await CustomerName.UpdateAsync(_ => result.Name, ct);
    }
}
```

## Data Loading Patterns

### 1. Initial Load
```csharp
public class CustomerListViewModel : BasePageViewModel
{
    public IFeed<IImmutableList<Customer>> Customers { get; }
    
    public CustomerListViewModel(ICustomerService service)
    {
        Customers = State<IImmutableList<Customer>>.Async(
            this, 
            ct => service.GetAllCustomersAsync(ct),
            ImmutableList<Customer>.Empty);
    }
}
```

### 2. Refresh Pattern
```csharp
public IState<Option<IImmutableList<Customer>>> CustomersState => 
    State<Option<IImmutableList<Customer>>>.Value(this, () => Option<IImmutableList<Customer>>.None());

[RelayCommand]
private async Task RefreshAsync(CancellationToken ct)
{
    await CustomersState.UpdateAsync(async _ => 
    {
        var customers = await _service.GetAllCustomersAsync(ct);
        return Option.Some(customers);
    }, ct);
}
```

## Validation Patterns

### 1. Property Validation
```csharp
public IState<string> Email => State<string>.Value(this, () => string.Empty);
public IFeed<Option<string>> EmailError => Email.Select(ValidateEmail);

private Option<string> ValidateEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        return Option.Some("Email is required");
    
    if (!IsValidEmailFormat(email))
        return Option.Some("Invalid email format");
    
    return Option<string>.None();
}
```

### 2. Form Validation
```csharp
public IFeed<bool> HasErrors => EmailError
    .CombineLatest(NameError, PhoneError, 
        (email, name, phone) => email.IsSome() || name.IsSome() || phone.IsSome());

public IFeed<bool> CanSubmit => HasErrors.Select(hasErrors => !hasErrors);
```

## Disposal Patterns

### 1. Automatic Disposal
```csharp
public partial class CustomerViewModel : BaseItemViewModel
{
    // States are automatically disposed when ViewModel is disposed
    public IState<string> Name => State<string>.Value(this, () => string.Empty);
}
```

### 2. Manual Subscription Disposal
```csharp
public CustomerViewModel()
{
    // Manual subscriptions need explicit disposal
    _customerService.CustomerUpdated
        .Subscribe(OnCustomerUpdated)
        .DisposeWith(this); // Disposed when ViewModel is disposed
}
```

## Testing ViewModels

### 1. Test Structure
```csharp
[TestClass]
public class CustomerViewModelTests
{
    private Mock<ICustomerService> _mockService;
    private CustomerViewModel _viewModel;
    
    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ICustomerService>();
        _viewModel = new CustomerViewModel(_mockService.Object);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _viewModel.Dispose();
    }
}
```

### 2. Testing Reactive Properties
```csharp
[TestMethod]
public async Task CustomerName_WhenUpdated_NotifiesCanSaveCommand()
{
    // Arrange
    var canExecuteChanged = false;
    _viewModel.SaveCommand.CanExecuteChanged += (s, e) => canExecuteChanged = true;
    
    // Act
    await _viewModel.CustomerName.UpdateAsync(_ => "New Name", CancellationToken.None);
    
    // Assert
    Assert.IsTrue(canExecuteChanged);
    Assert.IsTrue(_viewModel.SaveCommand.CanExecute(null));
}
```

## Best Practices

### Do's
- ✅ Keep ViewModels UI-agnostic
- ✅ Use dependency injection for all services
- ✅ Implement IDisposable when needed
- ✅ Use reactive patterns for complex state management
- ✅ Keep business logic in services, not ViewModels

### Don'ts
- ❌ Don't reference UI elements in ViewModels
- ❌ Don't use static services or singletons
- ❌ Don't forget to test async operations
- ❌ Don't put navigation logic in Views
- ❌ Don't create tight coupling between ViewModels