using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi.Startup
{
    internal static class IdentityExtensions
    {
        public static RouteHandlerBuilder FixOpenApiErrorShinyMediator(this WebApplication app)
        {
            return app.MapGet(
                    "/confirmEmail",
                    async (
                        [FromQuery] string userId,
                        [FromQuery] string code,
                        [FromQuery] string? changedEmail,
                        UserManager<AppUser> userManager
                    ) =>
                    {
                        var user = await userManager.FindByIdAsync(userId);
                        if (user == null)
                            return Results.NotFound();

                        var result = await userManager.ConfirmEmailAsync(user, code);
                        if (!result.Succeeded)
                            return Results.BadRequest(result.Errors);

                        return Results.Ok();
                    }
                )
                .WithName("ConfirmEmail") // <-- this sets operationId
                .WithTags("WebApi")
                .WithOpenApi();
        }
    }
}
