namespace WebApi.Mediator.Requests.Settings
{
    internal class UpdateCallSettingsRequest : ICommand
    {
        public int DefaultCallbackDays { get; set; }
    }
}