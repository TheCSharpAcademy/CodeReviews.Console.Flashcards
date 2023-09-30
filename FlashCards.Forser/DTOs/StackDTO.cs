namespace FlashCards.Forser.DTOs
{
    public class StackDTO
    {
        public string? Name { get; set; }
        public List<FlashCardDTO>? FlashCards { get; set; } = new List<FlashCardDTO>();

        public StackDTO() { }
        public StackDTO(string? name) 
        {
            Name = name;
        }
        public StackDTO(string? name, List<FlashCardDTO>? flashCards)
        {
            Name = name;
            FlashCards = flashCards;
        }
    }
}
