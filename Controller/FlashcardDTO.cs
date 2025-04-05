
class FlashcardDTO(Flashcard flashcard)
{
    public int Id { get; } = flashcard.Id;
    public string Front { get; } = flashcard.Front;
    public string Back { get; } = flashcard.Back;
}