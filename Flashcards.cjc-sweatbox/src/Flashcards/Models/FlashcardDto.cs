using Flashcards.Data.Entities;

namespace Flashcards.Models;

/// <summary>
/// Presentation version of the Flashcard object.
/// </summary>
public class FlashcardDto
{
    #region Constructors

    public FlashcardDto(FlashcardEntity entity)
    {
        Id = entity.Id;
        StackId = entity.StackId;
        Question = entity.Question;
        Answer = entity.Answer;
    }

    public FlashcardDto(int stackId, string question, string answer)
    {
        StackId = stackId;
        Question = question;
        Answer = answer;
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public int StackId { get; init; }

    public string Question { get; init; }

    public string Answer { get; init; }

    #endregion
}
