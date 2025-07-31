using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            
            // Seed test user
            var testEmail = "test@test.at";
            var existingUser = await userManager.FindByEmailAsync(testEmail);
            
            if (existingUser == null)
            {
                var user = new AppUser
                {
                    UserName = testEmail,
                    Email = testEmail,
                    EmailConfirmed = true,
                    DefaultCallbackDays = 3
                };
                
                var result = await userManager.CreateAsync(user, "Test123!");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}