using Microsoft.EntityFrameworkCore;
using SharedModels.Dtos.Contacts;

namespace WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContactDto> Contacts => Set<ContactDto>();
    }
}
