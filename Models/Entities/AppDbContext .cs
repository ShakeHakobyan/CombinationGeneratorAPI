using Microsoft.EntityFrameworkCore;

namespace CombinationGeneratorAPI.Models.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<RequestEntity> Requests { get; set; }


        public DbSet<CombinationEntity> Combinations { get; set; }

        public DbSet<CombinationItemEntity> CombinationItems { get; set; }
    }
}
