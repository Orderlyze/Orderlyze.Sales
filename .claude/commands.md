# Frequently Used Commands

## Build Commands
- `dotnet build` - Build entire solution
- `dotnet build src/WebApi/WebApi.csproj` - Build Web API only
- `dotnet build src/UnoApp/UnoApp/UnoApp.csproj` - Build Uno App only

## Run Commands
- `dotnet run --project src/WebApi/WebApi.csproj` - Run Web API (http://localhost:5062)
- `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj` - Run Uno App (http://localhost:5000)
- `dotnet run --project src/UnoApp/UnoApp/UnoApp.csproj --framework net9.0-desktop` - Run Desktop version

## Testing & Quality
- Run tests: Check README or search for test commands
- Lint: Check for `npm run lint` or similar commands
- Type check: Check for `npm run typecheck` or similar commands

## API Documentation
- Scalar UI: http://localhost:5062/scalar/v1 (when API is running)