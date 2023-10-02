namespace FlashCards.Forser.DTOs
{
    public class StackDto
    {
        public string? Name { get; set; }
        public List<FlashCardDto>? FlashCards { get; set; } = new List<FlashCardDto>();

        public StackDto() { }
        public StackDto(string? name) 
        {
            Name = name;
        }
        public StackDto(string? name, List<FlashCardDto>? flashCards)
        {
            Name = name;
            FlashCards = flashCards;
        }
    }
}
