using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shiny.Mediator;
using WebApi.Data;
using WebApi.Mediator.Requests.Settings;

namespace WebApi.Mediator.Handlers.Settings
{
    [SingletonHandler]
    internal class SettingsHandler(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) 
        : ICommandHandler<UpdateCallSettingsRequest>
    {
        [MediatorHttpPost("settings/updateCallSettings", "", RequiresAuthorization = true)]
        public async Task Handle(UpdateCallSettingsRequest request, IMediatorContext context, CancellationToken ct)
        {
            var userId = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }
            
            var user = await userManager.FindByNameAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.DefaultCallbackDays = request.DefaultCallbackDays;
            await userManager.UpdateAsync(user);
        }
    }
}