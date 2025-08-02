# Uno.Reactive Programming Guidelines

## Overview
This document provides detailed guidelines for using Uno.Reactive in the Orderlyze.Sales application alongside MVVM patterns.

## Core Concepts

### 1. State Management
- **IState<T>**: Use for mutable state that needs to be observed
- **IFeed<T>**: Use for read-only observable data streams
- **StateExtensions**: Leverage extension methods for state manipulation

### 2. Creating Reactive Properties

```csharp
// In ViewModel
public IState<string> SearchQuery => State<string>.Value(this, () => string.Empty);
public IFeed<IImmutableList<Customer>> FilteredCustomers => SearchQuery
    .Select(query => ApplyFilter(query))
    .AsAsync();
```

### 3. Combining MVVM with Reactive

```csharp
public partial class CustomerViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;
    
    // Reactive state
    public IState<bool> IsLoading => State<bool>.Value(this, () => false);
    public IState<string> SearchTerm => State<string>.Value(this, () => string.Empty);
    
    // Reactive feed from state
    public IFeed<IImmutableList<Customer>> Customers => SearchTerm
        .SelectAsync(async (term, ct) => await _customerService.SearchAsync(term, ct))
        .AsAsync();
    
    // Traditional MVVM command
    public ICommand RefreshCommand => new AsyncRelayCommand(RefreshAsync);
}
```

### 4. Best Practices

#### State Initialization
- Initialize states in constructor or as field initializers
- Use `State<T>.Value()` for simple values
- Use `State<T>.Async()` for async-loaded data

#### Data Flow
- Keep data flow unidirectional
- Use `Select`, `Where`, `CombineLatest` for transformations
- Prefer immutable data structures

#### Memory Management
- States are automatically disposed with the ViewModel
- For manual subscriptions, use `DisposeWith()` extension
- Avoid circular dependencies in reactive chains

### 5. Common Patterns

#### Loading States
```csharp
public IState<bool> IsLoading => State<bool>.Value(this, () => false);

private async Task LoadDataAsync(CancellationToken ct)
{
    await IsLoading.UpdateAsync(_ => true, ct);
    try
    {
        // Load data
    }
    finally
    {
        await IsLoading.UpdateAsync(_ => false, ct);
    }
}
```

#### Filtered Lists
```csharp
public IState<string> Filter => State<string>.Value(this, () => string.Empty);
public IFeed<IImmutableList<Item>> AllItems => State<IImmutableList<Item>>.Async(this, LoadItemsAsync);

public IFeed<IImmutableList<Item>> FilteredItems => 
    AllItems.CombineLatest(Filter, (items, filter) => 
        items.Where(i => i.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
             .ToImmutableList());
```

#### Form Validation
```csharp
public IState<string> Email => State<string>.Value(this, () => string.Empty);
public IFeed<bool> IsEmailValid => Email.Select(e => IsValidEmail(e));
public IFeed<bool> CanSubmit => IsEmailValid.CombineLatest(IsLoading, (valid, loading) => valid && !loading);
```

### 6. Testing Reactive ViewModels

```csharp
[TestMethod]
public async Task FilteredCustomers_WhenSearchTermChanges_ReturnsFilteredResults()
{
    // Arrange
    var viewModel = new CustomerViewModel(mockService);
    
    // Act
    await viewModel.SearchTerm.UpdateAsync(_ => "test", CancellationToken.None);
    var customers = await viewModel.Customers.GetAsync(CancellationToken.None);
    
    // Assert
    Assert.AreEqual(expectedCount, customers.Count);
}
```

### 7. UI Binding

```xml
<!-- Using x:Bind with Reactive -->
<TextBox Text="{x:Bind ViewModel.SearchTerm.Value, Mode=TwoWay}" />
<ProgressRing IsActive="{x:Bind ViewModel.IsLoading.Value, Mode=OneWay}" />
<ListView ItemsSource="{x:Bind ViewModel.FilteredItems.Value, Mode=OneWay}" />
```

### 8. Error Handling

```csharp
public IFeed<Option<string>> ErrorMessage => State<Option<string>>.Value(this, () => Option<string>.None());

public IFeed<IImmutableList<Customer>> Customers => State<IImmutableList<Customer>>
    .Async(this, async ct =>
    {
        try
        {
            return await _service.GetCustomersAsync(ct);
        }
        catch (Exception ex)
        {
            await ErrorMessage.UpdateAsync(_ => Option.Some(ex.Message), ct);
            return ImmutableList<Customer>.Empty;
        }
    });
```

## Do's and Don'ts

### Do's
- ✅ Use IState for mutable state
- ✅ Use IFeed for derived/computed values
- ✅ Keep reactive chains simple and readable
- ✅ Use async operations with proper cancellation
- ✅ Leverage immutable collections

### Don'ts
- ❌ Don't create circular dependencies
- ❌ Don't mix imperative and reactive unnecessarily
- ❌ Don't forget to handle errors in async feeds
- ❌ Don't create new states in methods (initialize in constructor)
- ❌ Don't use mutable collections in feeds

## Migration Guide

When migrating existing MVVM code to use Reactive:

1. Replace `ObservableCollection<T>` with `IState<IImmutableList<T>>`
2. Replace property setters with `State.UpdateAsync()`
3. Replace `PropertyChanged` events with reactive feeds
4. Keep commands as-is, but update their implementation to work with states