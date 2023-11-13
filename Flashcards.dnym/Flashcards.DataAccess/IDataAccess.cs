using Flashcards.DataAccess.DTOs;

namespace Flashcards.DataAccess;

public interface IDataAccess
{
    Task<int> CountStacksAsync(int? take = null, int skip = 0);
    Task<List<StackListItem>> GetStackListAsync(int? take = null, int skip = 0);
    Task<StackListItem> GetStackListItemByIdAsync(int id);
    Task<StackListItem?> GetStackListItemBySortNameAsync(string sortName);
    Task<StackListItem> GetStackListItemByFlashcardIdAsync(int flashcardId);
    Task CreateStackAsync(NewStack stack);
    Task DeleteStackAsync(int stackId);
    Task RenameStackAsync(int oldStackId, NewStack updatedStack);

    Task<int> CountFlashcardsAsync(int stackId);
    Task<bool> CardInStack(int stackId, int flashcardId);
    Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId, int? take = null, int skip = 0);
    Task<ExistingFlashcard> GetFlashcardByIdAsync(int id);
    Task CreateFlashcardAsync(NewFlashcard flashcard);
    Task UpdateFlashcardAsync(ExistingFlashcard flashcard);
    Task MoveFlashcardAsync(int flashcardId, int newStackId);
    Task DeleteFlashcardAsync(int id);

    Task<int> CountHistoryAsync();
    Task<List<HistoryListItem>> GetHistoryListAsync(int? take = null, int skip = 0);
    Task AddHistoryAsync(NewHistory history);
    Task<List<ExistingStudyResult>> GetStudyResults(int historyId, int? take = null, int skip = 0);
}
