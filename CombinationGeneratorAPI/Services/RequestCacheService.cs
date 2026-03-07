using CombinationGeneratorAPI.Data;
using CombinationGeneratorAPI.Models;
using CombinationGeneratorAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CombinationGeneratorAPI.Services
{
    public class RequestCacheService : IRequestCacheService
    {
        private readonly AppDbContext _db;

        public RequestCacheService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<RequestEntity?> GetExistingRequestAsync(GenerateRequest request)
        {
            var inputKey = JsonSerializer.Serialize(request);

            return await _db.Requests
                .Include(r => r.Combinations)
                    .ThenInclude(c => c.Items)
                .FirstOrDefaultAsync(r => r.InputItems == inputKey);
        }
    }
}