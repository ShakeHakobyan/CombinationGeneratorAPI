namespace CombinationGeneratorAPI.Controllers
{
    using CombinationGeneratorAPI.Models;
    using CombinationGeneratorAPI.Models.Entities;
    using CombinationGeneratorAPI.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class GenerateController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpPost]
        public async Task<ActionResult<GenerateResponse>> Generate([FromBody] GenerateRequest request)
        {
            if (request.Length <= 0)
                return BadRequest("Length must be greater than 0");

            var combos = CombinationGenerator.GenerateCombinations(
                request.Items,
                request.Length
            );

            var requestEntity = new RequestEntity
            {
                InputItems = System.Text.Json.JsonSerializer.Serialize(request.Items),
                CreatedAt = DateTime.UtcNow,
                Combinations = []
            };

            foreach (var combo in combos)
            {
                var combinationEntity = new CombinationEntity
                {
                    Items = combo.Select(item => new CombinationItemEntity
                    {
                        Item = item
                    }).ToList()
                };

                requestEntity.Combinations.Add(combinationEntity);
            }

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

            var response = new GenerateResponse
            {
                Id = requestEntity.Id,
                Combination = combos
            };

            return Ok(response);
        }
    }
}
