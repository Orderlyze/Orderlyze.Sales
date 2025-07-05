namespace UnoApp.Services.Example;

public interface INotificationService
{
    Task SendNotificationAsync(string message, CancellationToken cancellationToken = default);
}