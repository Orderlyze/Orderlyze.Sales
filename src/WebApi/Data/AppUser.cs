using Microsoft.AspNetCore.Identity;

namespace WebApi.Data
{
    public class AppUser : IdentityUser
    {
        public int DefaultCallbackDays { get; set; } = 3; // Default: 3 days later
    }
}
