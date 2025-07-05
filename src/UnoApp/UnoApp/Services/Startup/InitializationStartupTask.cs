using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shiny.Extensions.DependencyInjection;

namespace UnoApp.Services.Startup;

[Service(ServiceLifetime.Singleton)]
public class InitializationStartupTask : IStartupTask
{
    private readonly ILogger<InitializationStartupTask> _logger;

    public InitializationStartupTask(ILogger<InitializationStartupTask> logger)
    {
        _logger = logger;
    }

    public int Order => 1;

    public Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application initialization started");
        
        // Perform any initialization tasks here
        // For example: database initialization, cache warming, etc.
        
        _logger.LogInformation("Application initialization completed");
        return Task.CompletedTask;
    }
}