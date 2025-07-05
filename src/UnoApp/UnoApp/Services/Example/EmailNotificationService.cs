using Microsoft.Extensions.Logging;
using Shiny.Extensions.DependencyInjection;

namespace UnoApp.Services.Example;

[Service(ServiceLifetime.Scoped, "Email")]
public class EmailNotificationService : INotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(ILogger<EmailNotificationService> logger)
    {
        _logger = logger;
    }

    public Task SendNotificationAsync(string message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending email notification: {Message}", message);
        // Email sending logic here
        return Task.CompletedTask;
    }
}