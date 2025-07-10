# Code Generation Workflow

## WebApi.json to Client Requests Generation

This project uses automated code generation for API client requests.

### Step-by-Step Process

1. **Build WebApi Project**
   ```bash
   dotnet build src/WebApi/WebApi.csproj
   ```
   - Triggers `Microsoft.Extensions.ApiDescription.Server` package
   - Generates `WebApi.json` in WebApi project root
   - Configuration: `<OpenApiDocumentsDirectory>.</OpenApiDocumentsDirectory>`

2. **Copy WebApi.json to UnoApp**
   ```bash
   cp src/WebApi/WebApi.json src/UnoApp/UnoApp/WebApi.json
   ```
   - Manual copy required (no automatic sync)
   - This file feeds Shiny.Mediator's source generators

3. **Shiny Mediator Code Generation**
   - Configuration in UnoApp.csproj:
   ```xml
   <MediatorHttp Include="WebApi.json" 
                 Namespace="UnoApp.ApiClient" 
                 ContractPostfix="HttpRequest" 
                 Visible="true" />
   ```
   - Generates strongly-typed request classes
   - Namespace: `UnoApp.ApiClient`
   - Request suffix: `HttpRequest`
   - Generated file location: `/src/UnoApp/UnoApp/obj/generated/Shiny.Mediator.SourceGenerators/Shiny.Mediator.SourceGenerators.MediatorHttpRequestSourceGenerator/UnoApp.ApiClient.g.cs`

### Example Workflow
1. Add new endpoint to WebApi
2. Build WebApi project
3. Copy updated WebApi.json to UnoApp
4. Build UnoApp (generates request classes)