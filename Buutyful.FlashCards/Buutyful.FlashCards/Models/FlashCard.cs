using System.Runtime.CompilerServices;

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

    private FlashCardCreateDto(int deckId, string question, string back) =>
        (DeckId, FrontQuestion, BackAnswer) = (deckId, question, back);
    public static FlashCardCreateDto Create(int deckId, string question, string answer) =>
        new(deckId, question, answer);
    public static FlashCard ToCard(FlashCardCreateDto dto) =>
        new()
        {
            DeckId = dto.DeckId,
            FrontQuestion = dto.FrontQuestion,
            BackAnswer = dto.BackAnswer,
        };
}