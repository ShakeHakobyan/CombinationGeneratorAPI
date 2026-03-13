using CombinationGeneratorAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CombinationGeneratorAPI.Services
{
    public class LazyRequestCleanupService : IRequestCleanupService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<LazyRequestCleanupService> _logger;
        private const int MaxRequests = 1000;

        public LazyRequestCleanupService(AppDbContext db, ILogger<LazyRequestCleanupService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task PerformCleanupAsync()
        {
            try
            {
                int totalRequests = await _db.Requests.CountAsync();
                _logger.LogDebug("Total requests in DB before cleanup: {TotalRequests}", totalRequests);

                if (totalRequests <= MaxRequests)
                {
                    _logger.LogDebug("No cleanup needed. Total requests ({TotalRequests}) <= MaxRequests ({MaxRequests})",
                        totalRequests, MaxRequests);
                    return;
                }

                var oldRequests = await _db.Requests
                    .OrderByDescending(r => r.CreatedAt)
                    .Skip(MaxRequests)
                    .ToListAsync();

                if (oldRequests.Any())
                {
                    _logger.LogInformation("Cleaning up {Count} old requests", oldRequests.Count);
                    _db.Requests.RemoveRange(oldRequests);
                    await _db.SaveChangesAsync();
                    _logger.LogInformation("Cleanup completed successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during lazy request cleanup");
                throw;
            }
        }
    }
}