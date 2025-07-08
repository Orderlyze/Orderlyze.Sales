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
  /UnoApp/UnoApp    - Uno Platform client
  /External/WixApi  - Wix integration
  /Common           - Shared models and constants
```

## Recent Work Patterns
- Environment-specific configuration (appsettings.{environment}.json)
- Platform-specific behavior using runtime checks
- Mediator pattern for API requests
- Constants for navigation and region names

## Change Tracking
All changes made by Claude are automatically logged to:
- `/claude-changes.json` - Structured JSON format for learning
- `/claude-changes.log` - Human-readable log (deprecated)

## Notes for Future Sessions
- The project uses .NET 9
- WebAssembly builds require special handling for environment variables
- Git submodules are used (especially for mediator)
- Scalar is used for API documentation