using Microsoft.EntityFrameworkCore;
using SmartAccess.Domain.Entities;

namespace SmartAccess.Infrastructure.Persistence
{
    public class SmartAccessDbContext : DbContext
    {
        public SmartAccessDbContext(DbContextOptions<SmartAccessDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
    }
}
