namespace CombinationGeneratorAPI.Models
{
    public class GenerateRequest
    {
        public List<int> Items { get; set; } = [];

        public int Length { get; set; }
    }
}