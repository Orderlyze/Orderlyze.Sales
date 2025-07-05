# Orderlyze.Sales Project Context

## Project Overview
- **Repository**: github.com/Orderlyze/Orderlyze.Sales
- **Type**: Uno Platform application with Wix API integration
- **Architecture**: MVVM with Shiny.Mediator for CQRS pattern

## Key Technical Details

### Environment Configuration
- API keys are loaded from environment variables only (not from appsettings)
- Uses DotEnvLoader service for local development
- Compatible with GitHub Secrets for CI/CD
- API Key pattern: `WixApi__ApiKey` or `WIXAPI__APIKEY`

### Recent Changes
1. Added mediator submodule from github.com/Codelisk/mediator
2. Replaced Shiny.Mediator.Uno NuGet package with local submodule reference
3. Fixed SDK version compatibility (updated to Uno SDK 6.0.130)
4. Implemented secure API key configuration
5. Created clean DotEnvLoader service for environment loading
6. Fixed ContactListView scrolling by using ListView instead of ItemsRepeater
7. Created ContactsPage with ViewModel to display contacts from GetAllContactsRequest
8. Updated navigation: "Products" menu now opens ContactsPage, added "Wix Contacts" menu item

### Project Structure
- `/workspace/src/UnoApp/` - Main Uno application
- `/workspace/src/External/WixApi/` - Wix API integration
- `/workspace/src/WebApi/` - Backend API
- `/workspace/submodules/mediator/` - Shiny.Mediator submodule

### Common Issues & Solutions
1. **Scrolling not working**: Replace StackPanel with Grid in parent containers
2. **API key not loading**: Ensure .env file exists with `WixApi__ApiKey=YOUR_KEY`
3. **Build errors**: Check SDK versions match across all projects
4. **NavigationView pages not switching**: Do NOT use `uen:Region.Attached="True"` on the container Grid - only use `uen:Region.Navigator="Visibility"`

## User Preferences
- Prefers clean, centralized code architecture
- Dislikes "ugly" inline implementations
- Wants environment configuration in HostBuilderExtensions
- Uses GitHub Secrets for production deployments

## Technology Stack
- **Uno Platform**: Cross-platform UI framework
- **.NET 8.0**: Core framework
- **Shiny.Mediator**: CQRS pattern implementation
- **Wix API**: External API integration
- **Azure Services**: Can leverage Azure MCP Server for cloud resources

## Documentation Resources
- Microsoft Learn documentation accessible via Azure MCP Server
- Uno Platform documentation for cross-platform development
- .NET documentation for C# and framework features