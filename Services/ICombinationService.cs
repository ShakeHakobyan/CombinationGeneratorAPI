using CombinationGeneratorAPI.Models;

namespace CombinationGeneratorAPI.Services
{
    public interface ICombinationService
    {
        Task<GenerateResponse> GenerateAsync(GenerateRequest request);
    }
}
