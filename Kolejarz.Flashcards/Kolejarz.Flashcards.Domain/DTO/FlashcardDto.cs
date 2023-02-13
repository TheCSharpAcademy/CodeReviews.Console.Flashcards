namespace Kolejarz.Flashcards.Domain.DTO;

public record FlashcardDto(string FrontSide, string BackSide)
{
    public override string ToString() => FrontSide;
}
