namespace CombinationGeneratorAPI.Models.Entities
{
    public class CombinationEntity
    {
        public int Id { get; set; }

        public int RequestEntityId { get; set; }

        public RequestEntity? Request { get; set; }

        public List<CombinationItemEntity> Items { get; set; } = [];
    }
}
