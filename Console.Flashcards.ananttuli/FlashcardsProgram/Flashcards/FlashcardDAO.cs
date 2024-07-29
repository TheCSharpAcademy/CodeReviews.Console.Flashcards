namespace FlashcardsProgram.Flashcards;

public class FlashcardDao(
    int id,
    string front,
    string back,
    int stackId
)
{
    public int Id { get; set; } = id;
    public string Front { get; set; } = front;
    public string Back { get; set; } = back;
    public int StackId { get; set; } = stackId;

    public static string TableName
    {
        get
        {
            return "Flashcards";
        }
    }

    public override string ToString()
    {
        return $"{Front}";
    }
}