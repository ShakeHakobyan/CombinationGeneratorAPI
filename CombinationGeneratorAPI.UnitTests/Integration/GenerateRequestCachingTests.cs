using CombinationGeneratorAPI.Models;
using System.Net.Http.Json;

namespace CombinationGeneratorAPI.UnitTests.Integration
{
    public class GenerateRequestCachingTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;

        public GenerateRequestCachingTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Generate_ShouldReturnSameId_ForSameInputAndLength()
        {
            var request1 = new GenerateRequest
            {
                Items = new List<int> { 1, 2, 1 },
                Length = 2
            };

            var request2 = new GenerateRequest
            {
                Items = new List<int> { 1, 2, 1 },
                Length = 2
            };

            var response1 = await _fixture.Client.PostAsJsonAsync("/generate", request1);
            response1.EnsureSuccessStatusCode();
            var result1 = await response1.Content.ReadFromJsonAsync<GenerateResponse>();

            var response2 = await _fixture.Client.PostAsJsonAsync("/generate", request2);
            response2.EnsureSuccessStatusCode();
            var result2 = await response2.Content.ReadFromJsonAsync<GenerateResponse>();

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(result1.Id, result2.Id);
        }

        [Fact]
        public async Task Generate_ShouldReturnDifferentId_ForDifferentInputOrLength()
        {
            var baseRequest = new GenerateRequest
            {
                Items = new List<int> { 1, 0, 1, 5 },
                Length = 2
            };

            var differentInput = new GenerateRequest
            {
                Items = new List<int> { 2, 0, 1, 5 },
                Length = 2
            };

            var differentLength = new GenerateRequest
            {
                Items = new List<int> { 1, 0, 1, 5 },
                Length = 3
            };

            var responseBase = await _fixture.Client.PostAsJsonAsync("/generate", baseRequest);
            responseBase.EnsureSuccessStatusCode();
            var resultBase = await responseBase.Content.ReadFromJsonAsync<GenerateResponse>();

            var responseDiffInput = await _fixture.Client.PostAsJsonAsync("/generate", differentInput);
            responseDiffInput.EnsureSuccessStatusCode();
            var resultDiffInput = await responseDiffInput.Content.ReadFromJsonAsync<GenerateResponse>();

            var responseDiffLen = await _fixture.Client.PostAsJsonAsync("/generate", differentLength);
            responseDiffLen.EnsureSuccessStatusCode();
            var resultDiffLen = await responseDiffLen.Content.ReadFromJsonAsync<GenerateResponse>();

            Assert.NotNull(resultBase);
            Assert.NotNull(resultDiffInput);
            Assert.NotNull(resultDiffLen);
            Assert.NotEqual(resultBase.Id, resultDiffInput.Id);
            Assert.NotEqual(resultBase.Id, resultDiffLen.Id);
        }

        [Fact]
        public async Task Generate_EdgeCases_ShouldReturnDifferentId_ForLogicallySameRequest()
        {
            var request1 = new GenerateRequest
            {
                Items = new List<int> { 0 },
                Length = 1
            };

            var request2 = new GenerateRequest
            {
                Items = new List<int> { 0, 0 },
                Length = 1
            };

            var response1 = await _fixture.Client.PostAsJsonAsync("/generate", request1);
            response1.EnsureSuccessStatusCode();
            var result1 = await response1.Content.ReadFromJsonAsync<GenerateResponse>();

            var response2 = await _fixture.Client.PostAsJsonAsync("/generate", request2);
            response2.EnsureSuccessStatusCode();
            var result2 = await response2.Content.ReadFromJsonAsync<GenerateResponse>();

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotEqual(result1.Id, result2.Id);
        }
    }
}