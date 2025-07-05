using Microsoft.Extensions.Logging;
using Shiny.Extensions.DependencyInjection;

namespace UnoApp.Services.Example;

[Service(ServiceLifetime.Scoped, "SMS")]
public class SmsNotificationService : INotificationService
{
    private readonly ILogger<SmsNotificationService> _logger;

    public SmsNotificationService(ILogger<SmsNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendNotificationAsync(string message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending SMS notification: {Message}", message);
        // SMS sending logic here
        return Task.CompletedTask;
    }
}