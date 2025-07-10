# Claude Context for Orderlyze.Sales

## Project Overview
This is the Orderlyze.Sales project - a cross-platform application built with:
- Uno Platform for the client app (WebAssembly, Desktop, Mobile)
- ASP.NET Core Web API backend
- Shiny.Mediator for CQRS pattern
- WixApi integration for e-commerce

## Important Commands
- Build: `dotnet build`
- Run Web API: `dotnet run --project src/WebApi/WebApi.csproj`
- Run Uno App (Desktop): `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj`
- API Documentation: http://localhost:5062/scalar/v1

## Key Project Structure
```
/src
  /WebApi           - ASP.NET Core backend
    /Controllers    - API controllers
    /Data          - Entity Framework context and models
    /Helpers       - Authentication and other helpers
    /Mediator      - Mediator handlers
    
  /UnoApp/UnoApp    - Uno Platform client
    /Mediator      - Mediator pattern implementation
      /Handlers    - Request handlers
        /Authentication - Auth-related handlers
      /Requests    - Request objects
        /Authentication - Auth-related requests
    /Navigation    - Navigation framework
    /Presentation  - UI layer
      /Common      - Shared UI components
      /Pages       - Page views and view models
        /Contacts  - Contacts management
        /Login     - Login page
        /Main      - Main page
        /WixContacts - Wix contacts
      /Views       - Reusable views
    /Services      - Business services
      /Authentication - Auth service and interfaces
      /Common      - Shared services
      /Configuration - Config services
      /Http        - HTTP decorators and handlers
    /Startup       - Application startup configuration
    
  /External/WixApi  - Wix integration
  /SharedModels     - Shared models between projects
```

## Recent Work Patterns
- Environment-specific configuration (appsettings.{environment}.json)
- Platform-specific behavior using runtime checks
- Mediator pattern for API requests and handlers
- Constants for navigation and region names
- Bearer authentication with automatic token refresh
- Persistent token storage using Shiny.Mediator caching
- Request/Handler separation following project structure

## Change Tracking
All changes made by Claude are automatically logged to:
- `/claude-changes.json` - Structured JSON format for learning
- `/claude-changes.log` - Human-readable log (deprecated)

## Code Generation Workflow (IMPORTANT)
### WebApi.json to Client Requests Generation
This project uses a special workflow for generating client-side requests from the WebApi:

1. **Build WebApi Project**: 
   ```bash
   dotnet build src/WebApi/WebApi.csproj
   ```
   - This triggers the `Microsoft.Extensions.ApiDescription.Server` package
   - Generates `WebApi.json` in the WebApi project root (configured via `<OpenApiDocumentsDirectory>.</OpenApiDocumentsDirectory>`)

2. **Copy WebApi.json to UnoApp**:
   - Manually copy `/src/WebApi/WebApi.json` to `/src/UnoApp/UnoApp/WebApi.json`
   - This file is consumed by Shiny.Mediator's source generators

3. **Shiny Mediator Code Generation**:
   - The UnoApp.csproj contains:
   ```xml
   <MediatorHttp Include="WebApi.json" 
                 Namespace="UnoApp.ApiClient" 
                 ContractPostfix="HttpRequest" 
                 Visible="true" />
   ```
   - This generates strongly-typed request classes like `AddContactRequest`
   - Generated code goes into the `UnoApp.ApiClient` namespace
   - Requests get the `HttpRequest` postfix
   - **Generated files location**: `/src/UnoApp/UnoApp/obj/generated/Shiny.Mediator.SourceGenerators/Shiny.Mediator.SourceGenerators.MediatorHttpRequestSourceGenerator/UnoApp.ApiClient.g.cs`

4. **Handler Implementation**:
   - Handlers are NOT generated - they must be manually implemented
   - Follow the existing pattern in `/src/UnoApp/UnoApp/Mediator/Handlers/`
   - Always add `[SingletonHandler]` attribute to handlers

**Example**: When adding a new API endpoint:
1. Add the endpoint to WebApi
2. Build WebApi project
3. Copy the updated WebApi.json to UnoApp
4. Build UnoApp - this generates the request class
5. Implement the handler manually

## Notes for Future Sessions
- The project uses .NET 9
- WebAssembly builds require special handling for environment variables
- Git submodules are used (especially for mediator)
- Scalar is used for API documentation
- **IMPORTANT**: Always add `[SingletonHandler]` attribute to all Mediator handlers (Request/Command handlers)
- Hooks are working and logging changes to claude-changes.json