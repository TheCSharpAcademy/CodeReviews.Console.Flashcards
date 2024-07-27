namespace Flashcards.Models;

public class CreateStackDto(string name, IEnumerable<CreateFlashcardDto> flashcards)
{
    public string Name { get; set; } = name;

    public IEnumerable<CreateFlashcardDto> Flashcards { get; set; } = flashcards;
}

