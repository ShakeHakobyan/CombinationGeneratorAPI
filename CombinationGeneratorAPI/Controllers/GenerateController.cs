using CombinationGeneratorAPI.Models;
using CombinationGeneratorAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CombinationGeneratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateController : ControllerBase
    {
        private readonly ICombinationService _service;
        private readonly ILogger<GenerateController> _logger;
        public GenerateController(ICombinationService service, ILogger<GenerateController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<GenerateResponse>> Generate([FromBody] GenerateRequest request)
        {
            _logger.LogInformation("Received combination generation request: {@Request}", request);

            try
            {
                var response = await _service.GenerateAsync(request);

                _logger.LogInformation(
                    "Successfully generated {Count} combinations by {ResponseId}",
                    response.Combination.Count,
                    response.Id
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating combinations for request {@Request}", request);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}