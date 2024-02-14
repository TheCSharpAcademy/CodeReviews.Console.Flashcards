namespace Buutyful.FlashCards.Models;


public class FlashCard
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public string FrontQuestion { get; set; } = null!;
    public string BackAnswers { get; set; } = null!;
    public int CorrectAnswer {  get; set; }
}

public record FlashCardDisplayDto(string FrontQuestion, List<string> BackAnswers)
{
    public static implicit operator FlashCardDisplayDto(FlashCard card) => 
        new(card.FrontQuestion, new List<string>(card.BackAnswers.Split("|||")));
}

public record FlashCardCreateDto
{
    public string FrontQuestion { get; private set; } 
    public List<string> BackAnswers { get; private set; } 
    public int CorrectAnswer { get; set;}   

    private FlashCardCreateDto() { }

    private FlashCardCreateDto(string question, List<string> list, int index) =>
        (FrontQuestion, BackAnswers, CorrectAnswer) = (question, list, index);
   

    public static FlashCardCreateDto Create(string question, int index, params string[] answers)
    {
        if(index < 0 || index > answers.Length) throw new ArgumentOutOfRangeException(nameof(index));
        return new(question, new List<string>(answers), index);
    }
    public string DbAnswersString() => string.Join("|||", BackAnswers);
}