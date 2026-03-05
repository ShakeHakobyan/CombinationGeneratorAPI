namespace CombinationGeneratorAPI.Controllers
{
    using CombinationGeneratorAPI.Models;
    using CombinationGeneratorAPI.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class GenerateController : ControllerBase
    {
        [HttpPost]
        public ActionResult<GenerateResponse> Generate([FromBody] GenerateRequest request)
        {
            if (request.Length <= 0) 
            { 
                return BadRequest("Length must be greater than 0"); 
            }
                

            var combos = CombinationGenerator.GenerateCombinations(
                request.Items,
                request.Length
            );

            var response = new GenerateResponse
            {
                Id = 1,
                Combination = combos
            };

            return Ok(response);
        }
    }
}
