using CombinationGeneratorAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombinationGeneratorAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<RequestEntity> Requests { get; set; }

        public DbSet<CombinationEntity> Combinations { get; set; }

        public DbSet<CombinationItemEntity> CombinationItems { get; set; }
    }
}
