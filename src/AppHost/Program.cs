using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject("webapi", "../WebApi/WebApi.csproj");

// UnoApp needs to be run separately as it targets specific platforms
// builder.AddProject("unoapp", "../UnoApp/UnoApp/UnoApp.csproj")
//        .WithReference(api);

builder.Build().Run();
