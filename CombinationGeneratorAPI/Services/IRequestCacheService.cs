using CombinationGeneratorAPI.Models;
using CombinationGeneratorAPI.Models.Entities;

namespace CombinationGeneratorAPI.Services
{
    public interface IRequestCacheService
    {
        Task<RequestEntity?> GetExistingRequestAsync(GenerateRequest request);

    }
}
