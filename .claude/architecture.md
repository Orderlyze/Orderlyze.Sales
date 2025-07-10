# Architecture & Patterns

## CQRS with Shiny.Mediator
- All API communication uses Mediator pattern
- Request/Handler separation
- `[SingletonHandler]` attribute required on all handlers in UnoApp

## Authentication Flow
- JWT bearer tokens for API authentication
- Automatic token refresh mechanism
- Persistent token storage via Shiny.Mediator caching

## Project Structure Patterns
- Environment-specific config: `appsettings.{environment}.json`
- Platform-specific behavior using runtime checks
- Constants for navigation and region names
- Request/Handler separation following project structure

## Key Conventions
- Handlers must have `[SingletonHandler]` attribute in UnoApp
- Generated requests use `HttpRequest` suffix
- API client code in `UnoApp.ApiClient` namespace
- Follow existing patterns in neighboring files