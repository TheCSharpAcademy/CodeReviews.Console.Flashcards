using Flashcards.Data.Managers;
using Flashcards.Models;

namespace Flashcards.Controllers;

/// <summary>
/// Controller for all interactions between the Flashcard model and entity.
/// </summary>
public class FlashcardController
{
    #region Fields

    private readonly SqlDataManager _dataManager;

    #endregion
    #region Constructors

    public FlashcardController(string connectionString)
    {
        _dataManager = new SqlDataManager(connectionString);
    }

    #endregion
    #region Methods

    public void AddFlashcard(int stackId, string question, string answer)
    {
        _dataManager.AddFlashcard(stackId, question, answer);
    }

    public void DeleteFlashcard(int id)
    {
        _dataManager.DeleteFlashcard(id);
    }

    public IReadOnlyList<FlashcardDto> GetFlashcards()
    {
        return _dataManager.GetFlashcards().Select(x => new FlashcardDto(x)).ToList();
    }

    public IReadOnlyList<FlashcardDto> GetFlashcards(int stackId)
    {
        return _dataManager.GetFlashcards(stackId).Select(x => new FlashcardDto(x)).ToList();
    }

    public void SetFlashcard(int id, string question, string answer)
    {
        _dataManager.SetFlashcard(id, question, answer);
    }

    #endregion
}
