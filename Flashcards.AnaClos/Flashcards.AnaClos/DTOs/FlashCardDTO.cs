namespace Flashcards.AnaClos.DTOs;

public class FlashCardDTO
{
    public int SequentialId { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
    public string StackName { get; set; }
    public static List<FlashCardDTO> Flashcards { get; set; } = new List<FlashCardDTO>();
}