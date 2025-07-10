using Microsoft.AspNetCore.Identity;
using Shiny.Mediator;
using WebApi.Data;
using WebApi.Mediator.Requests.Settings;

namespace WebApi.Mediator.Handlers.Settings
{
    [SingletonHandler]
    internal class SettingsHandler : ICommandHandler<UpdateCallSettingsRequest>
    {
        private readonly UserManager<AppUser> _userManager;

        public SettingsHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [MediatorHttpPost("settings/updateCallSettings", "", RequiresAuthorization = true)]
        public async Task Handle(UpdateCallSettingsRequest request, IMediatorContext context, CancellationToken ct)
        {
            var userId = context.HttpContext?.User?.Identity?.Name;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }
            
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.DefaultCallbackDays = request.DefaultCallbackDays;
            await _userManager.UpdateAsync(user);
        }
    }
}