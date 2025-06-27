using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.WebApi>("webapi");

builder.AddProject<Projects.UnoApp>("unoapp")
       .WithReference(api);

builder.AddDashboard();

builder.Build().Run();
