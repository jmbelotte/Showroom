using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ShowRoomDbContext : DbContext
    {
        public ShowRoomDbContext(DbContextOptions<ShowRoomDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShowRoomDbContext).Assembly);
        }
    }
}
