# Region-Based Navigation in Orderlyze.Sales

## Overview
Region-based navigation is a powerful pattern in Uno Platform that allows dynamic content loading within designated areas of a page. This document explains how to implement and use regions effectively in the Orderlyze.Sales application.

## What are Regions?
Regions are named content areas in your XAML views that can be dynamically populated with different UI content. They enable:
- Modular UI composition
- Dynamic content switching
- Separation of navigation logic from view structure
- Reusable view components

## Implementation Pattern

### 1. ViewModel - Defining Regions

In your PageViewModel, override the `GetRegions` method to define which regions your page will use:

```csharp
public class ContactsPageViewModel : BasePageViewModel
{
    public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
    {
        return [new("List", RegionViewsNames.ContactList, data: Contacts)];
    }
    
    public IFeed<IEnumerable<ContactsListModel>> Contacts =>
        Feed.Async(async ct =>
        {
            // Load and return data for the region
            var contacts = await _mediator.Request(new GetAllContactHttpRequest(), ct);
            return contacts.Result.Select(x => new ContactsListModel(
                x.Id,
                x.Name,
                x.Email,
                x.Phone,
                x.Industry
            ));
        });
}
```

### 2. View - Declaring Region Placeholders

In your XAML page, declare ContentControl elements with Region attached properties:

```xml
<Page x:Class="UnoApp.Presentation.Pages.Contacts.ContactsPage"
      xmlns:uen="using:Uno.Extensions.Navigation.UI">
    
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        
        <!-- Page Header -->
        <Border Grid.Row="0" Style="{StaticResource PageHeaderBorderStyle}">
            <TextBlock Text="Kontakte" Style="{StaticResource HeadingTextStyle}"/>
        </Border>
        
        <!-- Region Content Area -->
        <ContentControl Grid.Row="1" 
                        uen:Region.Attached="True" 
                        uen:Region.Name="List"/>
    </Grid>
</Page>
```

## RegionModel Parameters

The `RegionModel` constructor accepts several parameters:

```csharp
new RegionModel(
    regionName: "List",              // Must match Region.Name in XAML
    viewName: RegionViewsNames.ContactList,  // View to load
    data: Contacts,                  // Optional: Data to pass to the view
    route: null                      // Optional: Custom route
)
```

## Best Practices

### 1. Naming Conventions
- Use descriptive region names that indicate their purpose
- Store view names in constants (e.g., `RegionViewsNames.ContactList`)
- Keep region names consistent between ViewModel and View

### 2. Data Passing
```csharp
// Pass data through RegionModel
public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
{
    return [
        new("Header", RegionViewsNames.PageHeader, data: HeaderData),
        new("Content", RegionViewsNames.ContactList, data: Contacts),
        new("Footer", RegionViewsNames.PageFooter)
    ];
}
```

### 3. Multiple Regions
```xml
<Grid>
    <!-- Multiple regions in one page -->
    <ContentControl Grid.Row="0" 
                    uen:Region.Attached="True" 
                    uen:Region.Name="Header"/>
    
    <ContentControl Grid.Row="1" 
                    uen:Region.Attached="True" 
                    uen:Region.Name="Content"/>
    
    <ContentControl Grid.Row="2" 
                    uen:Region.Attached="True" 
                    uen:Region.Name="Sidebar"/>
</Grid>
```

### 4. Dynamic Region Updates
```csharp
public class DashboardViewModel : BasePageViewModel
{
    private readonly IState<string> _selectedView = State<string>.Value(this, () => "Overview");
    
    public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
    {
        var viewName = _selectedView.Value switch
        {
            "Overview" => RegionViewsNames.DashboardOverview,
            "Analytics" => RegionViewsNames.DashboardAnalytics,
            "Reports" => RegionViewsNames.DashboardReports,
            _ => RegionViewsNames.DashboardOverview
        };
        
        return [new("Main", viewName)];
    }
    
    public ICommand SwitchViewCommand => new AsyncRelayCommand<string>(async (view, ct) =>
    {
        await _selectedView.UpdateAsync(_ => view, ct);
        // Trigger region refresh
        await RefreshRegionsAsync(ct);
    });
}
```

## Common Patterns

### 1. List-Detail Pattern
```csharp
public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
{
    var regions = new List<RegionModel>
    {
        new("List", RegionViewsNames.CustomerList, data: Customers)
    };
    
    // Add detail region if a customer is selected
    if (SelectedCustomer.Value != null)
    {
        regions.Add(new("Detail", RegionViewsNames.CustomerDetail, data: SelectedCustomer));
    }
    
    return regions;
}
```

### 2. Tab-Based Navigation
```csharp
public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
{
    var activeTab = e.Route.TryGetParameter<string>("tab") ?? "contacts";
    
    var viewName = activeTab switch
    {
        "contacts" => RegionViewsNames.ContactList,
        "companies" => RegionViewsNames.CompanyList,
        "deals" => RegionViewsNames.DealList,
        _ => RegionViewsNames.ContactList
    };
    
    return [new("TabContent", viewName)];
}
```

### 3. Conditional Regions
```csharp
public override IEnumerable<RegionModel> GetRegions(NavigationEventArgs e)
{
    var regions = new List<RegionModel>();
    
    // Always show main content
    regions.Add(new("Main", RegionViewsNames.MainContent));
    
    // Conditionally add regions based on user permissions
    if (UserHasPermission("ViewAnalytics"))
    {
        regions.Add(new("Analytics", RegionViewsNames.AnalyticsPanel));
    }
    
    if (IsInEditMode.Value)
    {
        regions.Add(new("Toolbar", RegionViewsNames.EditToolbar));
    }
    
    return regions;
}
```

## Region Views

Region views are typically UserControls that receive data through their DataContext:

```csharp
public partial class ContactListView : UserControl
{
    public ContactListView()
    {
        this.InitializeComponent();
    }
}
```

```xml
<UserControl x:Class="UnoApp.Presentation.Views.Contacts.ContactListView">
    <ListView ItemsSource="{x:Bind Data}">
        <!-- ListView content -->
    </ListView>
</UserControl>
```

## Troubleshooting

### Region Not Loading
1. Verify region name matches between ViewModel and XAML
2. Check that the view is registered in DI container
3. Ensure `uen:Region.Attached="True"` is set
4. Verify the view name constant is correct

### Data Not Appearing
1. Check that data is passed in RegionModel constructor
2. Verify data binding in the region view
3. Ensure async data is properly loaded

### Performance Considerations
- Regions are loaded on-demand
- Use lightweight views for frequently switched regions
- Consider caching for expensive data operations
- Dispose subscriptions properly in region views