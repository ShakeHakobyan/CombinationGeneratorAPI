namespace CombinationGeneratorAPI.Models
{
    public class GenerateResponse
    {
        public int Id { get; set; }

        public List<List<string>> Combination { get; set; } = [];
    }
}