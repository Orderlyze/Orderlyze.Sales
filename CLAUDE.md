# Orderlyze.Sales - AI Assistant Configuration

This project is a cross-platform sales management application with e-commerce integration.

## Quick Commands

### Build & Run
- Build solution: `dotnet build`
- Run API: `dotnet run --project src/WebApi/WebApi.csproj` (http://localhost:5062)
- Run App: `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj` (http://localhost:5000)
- API Docs: http://localhost:5062/scalar/v1

### Git & GitHub
- Create issue: `gh issue create`
- Create PR: `gh pr create`
- Feature branch: `git checkout -b feature/issue-XXX-description`

## Architecture Guidelines

### CQRS with Shiny.Mediator
- **CRITICAL**: All handlers in UnoApp MUST have `[SingletonHandler]` attribute
- Request/Handler separation pattern
- Generated requests use `HttpRequest` suffix

### Authentication
- JWT bearer tokens with automatic refresh
- Token storage via Shiny.Mediator caching
- Persistent authentication across sessions

### Code Generation Workflow
1. Build WebApi: `dotnet build src/WebApi/WebApi.csproj`
2. Copy spec: `cp src/WebApi/WebApi.json src/UnoApp/UnoApp/WebApi.json`
3. Build UnoApp to generate client requests
4. Generated namespace: `UnoApp.ApiClient`

## Development Workflow

### Feature Implementation
When implementing features (requests starting with "WORKFLOW:", "Implementiere:", or "Feature:"):
1. Create GitHub issue: `gh issue create`
2. Create feature branch: `git checkout -b feature/issue-XXX-name`
3. Implement changes following existing patterns
4. Build and fix errors: `dotnet build`
5. Commit with issue reference: `git commit -m "feat: Description (#XXX)"`
6. Create PR with "Closes #XXX" in body: `gh pr create`

### Code Style
- Follow existing patterns in neighboring files
- Check imports and dependencies before adding new ones
- Use environment-specific config: `appsettings.{environment}.json`
- Never commit secrets or API keys

## Project Structure
- `/src/WebApi` - ASP.NET Core backend
- `/src/UnoApp/UnoApp` - Uno Platform client
- `/src/External/WixApi` - Wix e-commerce integration
- `/src/SharedModels` - Shared models
- `/submodules/mediator` - Shiny.Mediator submodule

## Testing
- Check README for test commands
- Run linting if available
- Verify builds before committing

## Important Reminders
- Always add `[SingletonHandler]` to Mediator handlers in UnoApp
- Follow WebApi.json workflow for new endpoints
- Never create files unless necessary
- Prefer editing existing files over creating new ones