namespace Models;

public class CreateStackDto()
{
    public required string Name { get; set; }

    public required IEnumerable<CreateFlashcardDto> Flashcards { get; set; }
}

