namespace CombinationGeneratorAPI.Models.Entities
{
    public class RequestEntity
    {
        public int Id { get; set; }

        public string InputItems { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public List<CombinationEntity> Combinations { get; set; } = [];
    }
}