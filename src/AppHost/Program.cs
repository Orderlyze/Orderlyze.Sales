using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject("webapi", "../WebApi/WebApi.csproj");

builder.AddProject("unoapp", "../UnoApp/UnoApp/UnoApp.csproj")
       .WithReference(api);

builder.Build().Run();
