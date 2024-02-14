namespace Buutyful.FlashCards.Models;


public class FlashCard
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public string FrontQuestion { get; set; } = null!;
    public string BackAnswer { get; set; } = null!;    
}

public record FlashCardDisplayDto(string FrontQuestion, string BackAnswers)
{
    public static implicit operator FlashCardDisplayDto(FlashCard card) => 
        new(card.FrontQuestion, card.BackAnswer);
}

public record FlashCardCreateDto
{
    public int DeckId { get; private set; }
    public string FrontQuestion { get; private set; } 
    public string BackAnswer { get; private set; }    

    private FlashCardCreateDto() { }

    private FlashCardCreateDto(int deck, string question, string back) =>
        (DeckId, FrontQuestion, BackAnswer) = (deck, question, back);


    public static FlashCardCreateDto Create(int id, string question, string answer) =>
        new(id, question, answer);    
    
}