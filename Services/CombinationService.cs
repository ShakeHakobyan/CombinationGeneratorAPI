namespace CombinationGeneratorAPI.Services
{
    using CombinationGeneratorAPI.Data;
    using CombinationGeneratorAPI.Models;
    using CombinationGeneratorAPI.Models.Entities;
    using System.Text.Json;

    public class CombinationService(AppDbContext db) : ICombinationService
    {
        public async Task<GenerateResponse> GenerateAsync(GenerateRequest request)
        {
            GenerateRequestValidator.Validate(request);

            var combinations = CombinationGenerator.GenerateCombinations(request.Items, request.Length);

            var requestEntity = CreateRequestEntity(request, combinations);

            await SaveRequestAsync(requestEntity);

            return BuildResponse(requestEntity, combinations);
        }

        private RequestEntity CreateRequestEntity(GenerateRequest request, List<List<string>> combinations)
        {
            return new RequestEntity
            {
                InputItems = JsonSerializer.Serialize(request.Items),
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
            await using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                db.Requests.Add(requestEntity);
                await db.SaveChangesAsync();
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