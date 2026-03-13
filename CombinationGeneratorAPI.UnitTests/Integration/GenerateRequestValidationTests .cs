using CombinationGeneratorAPI.Models;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace CombinationGeneratorAPI.UnitTests.Integration
{
    public class GenerateRequestValidationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly IntegrationTestFixture _fixture;

        public GenerateRequestValidationTests(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Generate_ShouldReturnBadRequest_ForNegativeItems()
        {
            var request = new GenerateRequest
            {
                Items = [1, 2, -3],
                Length = 2
            };

            var response = await _fixture.Client.PostAsJsonAsync("/generate", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Generate_ShouldReturnBadRequest_WhenItemsCountExceeds26()
        {
            var request = new GenerateRequest
            {
                Items = Enumerable.Range(1, 28).ToList(),
                Length = 2
            };

            var response = await _fixture.Client.PostAsJsonAsync("/generate", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public async Task Generate_ShouldReturnBadRequest_WhenLengthIsNegative(int invalidLength)
        {
            var request = new GenerateRequest
            {
                Items = [1, 2, 3],
                Length = invalidLength
            };

            var response = await _fixture.Client.PostAsJsonAsync("/generate", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Generate_ShouldReturnOk_ForValidRequest()
        {
            var request = new GenerateRequest
            {
                Items = new List<int> { 0, 2, 0, 1, 1 },
                Length = 3
            };

            var response = await _fixture.Client.PostAsJsonAsync("/generate", request);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<GenerateResponse>();

            Assert.NotNull(result);
            Assert.NotEmpty(result.Combination);
        }
    }
}