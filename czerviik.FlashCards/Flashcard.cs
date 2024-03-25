namespace FlashCards;

public class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}

public class FlashcardSessionDto
{
    public string Question { get; set; }
    public string Answer {get; set;}
}

public class FlashcardReviewDto
{
    public int DisplayId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}