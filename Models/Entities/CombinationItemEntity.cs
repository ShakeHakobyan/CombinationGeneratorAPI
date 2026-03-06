namespace CombinationGeneratorAPI.Models.Entities
{
    public class CombinationItemEntity
    {
        public int Id { get; set; }

        public int CombinationEntityId { get; set; }

        public CombinationEntity? Combination { get; set; }

        public required string Item { get; set; }
    }
}
