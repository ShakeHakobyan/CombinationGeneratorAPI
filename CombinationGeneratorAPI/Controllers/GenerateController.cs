using CombinationGeneratorAPI.Models;
using CombinationGeneratorAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombinationGeneratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateController : ControllerBase
    {
        private readonly ICombinationService _service;

        public GenerateController(ICombinationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<GenerateResponse>> Generate([FromBody] GenerateRequest request)
        {
            var response = await _service.GenerateAsync(request);

            return Ok(response);
        }
    }
}