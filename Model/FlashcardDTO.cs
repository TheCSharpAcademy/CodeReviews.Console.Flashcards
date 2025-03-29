
class FlashcardDTO
{
    public int Id { get; }
    public string Front { get; } = "";
    public string Back { get; } = "";

    public FlashcardDTO(Flashcard flashcard)
    {
        Id = flashcard.Id;
        Front = flashcard.Front;
        Back = flashcard.Back;
    }
}