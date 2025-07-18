
namespace FlashCard.SheheryarRaza.Entities
{
    public class Stacks
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<FlashCards> FlashCards { get; set; } = new List<FlashCards>();
    }
}
