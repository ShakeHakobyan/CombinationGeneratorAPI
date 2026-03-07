using CombinationGeneratorAPI.Data;
using CombinationGeneratorAPI.Models;
using CombinationGeneratorAPI.Models.Entities;
using System.Text.Json;

namespace CombinationGeneratorAPI.Services
{
    public class CombinationService : ICombinationService
    {
        private readonly AppDbContext _db;
        private readonly IRequestCacheService _cacheService;
        private readonly IRequestCleanupService _cleanupService;

        public CombinationService(
            AppDbContext db,
            IRequestCacheService cacheService,
            IRequestCleanupService cleanupService)
        {
            _db = db;
            _cacheService = cacheService;
            _cleanupService = cleanupService;
        }
        public async Task<GenerateResponse> GenerateAsync(GenerateRequest request)
        {
            // Cleanup (strategy-agnostic)
            await _cleanupService.PerformCleanupAsync();

            // Check if request already exists in DB
            var existingRequest = await _cacheService.GetExistingRequestAsync(request);
            if (existingRequest != null)
            {
                return new GenerateResponse
                {
                    Id = existingRequest.Id,
                    Combination = existingRequest.Combinations
                        .Select(c => c.Items.Select(i => i.Item).ToList())
                        .ToList()
                };
            }

            // Generate new combinations
            var combinations = CombinationGenerator.Generate(request.Items, request.Length);

            // Create request entity with combinations
            var requestEntity = CreateRequestEntity(request, combinations);

            // Save to DB in a transaction
            await SaveRequestAsync(requestEntity);

            // Build and return response
            return BuildResponse(requestEntity, combinations);
        }

        private RequestEntity CreateRequestEntity(GenerateRequest request, List<List<string>> combinations)
        {
            return new RequestEntity
            {
                InputItems = JsonSerializer.Serialize(request),
                CreatedAt = DateTime.UtcNow,
                Combinations = combinations.Select(CreateCombinationEntity).ToList()
            };
        }

        private CombinationEntity CreateCombinationEntity(List<string> combo)
        {
            return new CombinationEntity
            {
                Items = combo.Select(item => new CombinationItemEntity { Item = item }).ToList()
            };
        }

        private async Task SaveRequestAsync(RequestEntity requestEntity)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Requests.Add(requestEntity);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static GenerateResponse BuildResponse(RequestEntity requestEntity, List<List<string>> combinations)
        {
            return new GenerateResponse
            {
                Id = requestEntity.Id,
                Combination = combinations
            };
        }
    }
}