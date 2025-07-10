# Project Structure

```
/src
  /WebApi               # ASP.NET Core backend
    /Controllers        # API controllers
    /Data              # Entity Framework context and models
    /Helpers           # Authentication and other helpers
    /Mediator          # Mediator handlers
      /Handlers        # Request handlers
      /Requests        # Request objects
    
  /UnoApp/UnoApp       # Uno Platform client
    /Mediator          # Mediator pattern implementation
      /Handlers        # Request handlers
        /Authentication
        /Contacts
      /Requests        # Request objects (manual only)
    /Navigation        # Navigation framework
    /Presentation      # UI layer
      /Common          # Shared UI components
      /Pages           # Page views and view models
        /Contacts      # Contacts management
        /Login         # Login page
        /Main          # Main page
        /WixContacts   # Wix contacts
      /Views           # Reusable views
    /Services          # Business services
      /Authentication  # Auth service and interfaces
      /Common          # Shared services
      /Configuration   # Config services
      /Http            # HTTP decorators and handlers
    /Startup           # Application startup configuration
    WebApi.json        # Generated API specification
    
  /External/WixApi     # Wix e-commerce integration
  /SharedModels        # Shared models between projects

/submodules
  /mediator            # Shiny.Mediator git submodule
```