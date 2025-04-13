namespace Flashcards.DAL.DTO
{
    public class StackDTO
    {
        public string Name { get; set; }
        public List<FlashcardStackDTO> FlashCards { get; set; }
    }
}
