using CombinationGeneratorAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CombinationGeneratorAPI.Services
{
    public class LazyRequestCleanupService : IRequestCleanupService
    {
        private readonly AppDbContext _db;
        private const int MaxRequests = 1000;

        public LazyRequestCleanupService(AppDbContext db)
        {
            _db = db;
        }

        public async Task PerformCleanupAsync()
        {
            int totalRequests = await _db.Requests.CountAsync();
            if (totalRequests <= MaxRequests)
            {
                return;
            }

            var oldRequests = await _db.Requests
                .OrderByDescending(r => r.CreatedAt)
                .Skip(MaxRequests)
                .ToListAsync();

            if (oldRequests.Any())
            {
                _db.Requests.RemoveRange(oldRequests);
                await _db.SaveChangesAsync();
            }
        }
    }
}