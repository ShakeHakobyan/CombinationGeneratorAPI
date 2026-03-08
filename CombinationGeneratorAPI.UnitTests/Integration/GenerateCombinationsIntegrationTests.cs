using System.Net.Http.Json;
using CombinationGeneratorAPI.Models;

namespace CombinationGeneratorAPI.UnitTests.Integration
{
    public class GenerateCombinationsIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;

        public GenerateCombinationsIntegrationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Generate_ShouldReturnAndStoreValidCombinations()
        {
            var request = new GenerateRequest
            {
                Items = new List<int> { 1, 2, 1 },
                Length = 2
            };

            var response = await _fixture.Client.PostAsJsonAsync("/generate", request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GenerateResponse>();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.NotEmpty(result.Combination);
        }
    }
}