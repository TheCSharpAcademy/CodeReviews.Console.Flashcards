namespace FlashCards.kwm0304.Dtos;

public class StackDto
{
  public int Id { get; set; }
  public string Name { get; set; }
  public List<FlashCardDto> Flashcards { get; set; }
  public StackDto(int id, string name, List<FlashCardDto> flashcards)
    {
        Id = id;
        Name = name;
        Flashcards = flashcards;
    }
}