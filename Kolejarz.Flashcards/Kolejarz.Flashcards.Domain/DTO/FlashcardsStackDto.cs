namespace Kolejarz.Flashcards.Domain.DTO;

public record FlashcardsStackDto(string Name, string Description, IEnumerable<FlashcardDto> Flashcards)
{
    public override string ToString() => Name;
}
