# Code Patterns and Best Practices

## Uno Platform Patterns

### NavigationView with Multiple Pages
```xml
<!-- Correct pattern - Use Region.Navigator="Visibility" WITHOUT Region.Attached on container -->
<NavigationView uen:Region.Attached="true">
    <NavigationView.MenuItems>
        <NavigationViewItem Content="Page1" uen:Region.Name="Page1" />
        <NavigationViewItem Content="Page2" uen:Region.Name="Page2" />
    </NavigationView.MenuItems>
    
    <Grid uen:Region.Navigator="Visibility">
        <ContentControl uen:Region.Name="Page1" uen:Region.Attached="true"/>
        <ContentControl uen:Region.Name="Page2" uen:Region.Attached="true"/>
    </Grid>
</NavigationView>
```
**Important**: Do NOT add `uen:Region.Attached="True"` to the container Grid - only use it on the NavigationView and ContentControls!

### Making Views Scrollable
```xml
<!-- Use ListView for built-in scrolling -->
<ListView ItemsSource="{Binding Data}"
          SelectionMode="None"
          IsItemClickEnabled="False">
    <ListView.ItemContainerStyle>
        <Style TargetType="ListViewItem">
            <Setter Property="HorizontalContentAlignment" Value="Stretch"/>
            <Setter Property="Padding" Value="0"/>
        </Style>
    </ListView.ItemContainerStyle>
</ListView>
```

### Environment Configuration
```csharp
// In HostBuilderExtensions.cs
internal static IHostBuilder AddEnvironment(this IHostBuilder hostBuilder)
{
    // Load environment variables from .env file if present
    DotEnvLoader.Load();
    
#if DEBUG
    hostBuilder = hostBuilder.UseEnvironment(Environments.Development);
#endif
    return hostBuilder;
}
```

### Service Registration Pattern
```csharp
// In ServicesExtensions.cs
public static IServiceCollection AddCommonServices(
    this IServiceCollection services, 
    IConfiguration configuration)
{
    services.AddShinyMediator(typeof(App).Assembly);
    services.AddWixApi();
    return services;
}
```

## Shiny.Extensions Patterns

### Service Registration with Attributes
```csharp
// Simple service registration
[Service(ServiceLifetime.Singleton)]
public class MyService : IMyService { }

// Keyed service registration
[Service(ServiceLifetime.Scoped, "MyKey")]
public class KeyedService : IMyService { }

// Multiple interface registration
[Service(ServiceLifetime.Singleton)]
public class MultiService : IService1, IService2 { }
```

### Startup Tasks
```csharp
[Service(ServiceLifetime.Singleton)]
public class MyStartupTask : IStartupTask
{
    public int Order => 1;
    public Task RunAsync(CancellationToken cancellationToken) { }
}
```

### Registration in DI
```csharp
services.AddGeneratedServices();
services.AddGeneratedStartupTasks();
await host.RunStartupTasks();
```

## Git Workflow
1. Always create feature branches for changes
2. Use descriptive commit messages
3. Include Claude attribution in commits:
   ```
   🤖 Generated with [Claude Code](https://claude.ai/code)
   
   Co-Authored-By: Claude <noreply@anthropic.com>
   ```

## Security Best Practices
- Never hardcode API keys in code or appsettings
- Use environment variables for sensitive data
- Add .env files to .gitignore
- Use GitHub Secrets for CI/CD