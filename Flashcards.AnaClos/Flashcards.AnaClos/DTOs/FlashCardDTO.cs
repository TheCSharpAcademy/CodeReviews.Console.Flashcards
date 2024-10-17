namespace Flashcards.AnaClos.DTOs;

public class FlashCardDto
{
    public int SequentialId { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
    public string StackName { get; set; }
    public static List<FlashCardDto> Flashcards { get; set; } = new List<FlashCardDto>();
}