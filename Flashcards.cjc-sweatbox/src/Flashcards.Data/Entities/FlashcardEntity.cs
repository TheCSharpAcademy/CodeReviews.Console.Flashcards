using System.Data;
using Flashcards.Data.Extensions;

namespace Flashcards.Data.Entities;

/// <summary>
/// Database version of the Flashcard object.
/// </summary>
public class FlashcardEntity
{
    #region Constructors

    public FlashcardEntity(IDataReader reader)
    {
        Id = reader.GetInt32("Id");
        StackId = reader.GetInt32("StackId");
        Question = reader.GetString("Question");
        Answer = reader.GetString("Answer");
    }

    #endregion
    #region Properties

    public int Id { get; init; }

    public int StackId { get; init; }

    public string Question { get; init; }

    public string Answer { get; init; }

    #endregion
}
