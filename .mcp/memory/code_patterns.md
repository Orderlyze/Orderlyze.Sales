# Code Patterns and Best Practices

## Uno Platform Patterns

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