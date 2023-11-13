using Flashcards.DataAccess.DTOs;
using Flashcards.DataAccess.MockDB.Models;

namespace Flashcards.DataAccess.MockDB;

public class MockDB : IDataAccess
{
    private readonly List<StacksRow> _stacks = new();
    private readonly List<FlashcardsRow> _flashcards = new();
    private readonly List<HistoryRow> _history = new();
    private readonly List<HistoryToFlashcardRow> _historyToFlashcard = new();

    public Task<int> CountStacksAsync(int? take = null, int skip = 0)
    {
        if (take == null)
        {
            return Task.FromResult(_stacks.Skip(skip).Count());
        }
        return Task.FromResult(_stacks.Skip(skip).Take((int)take).Count());
    }

    private StackListItem RowToStackListItem(StacksRow sr)
    {
        DateTime? lastStudied = null;
        if (_history.Any(hr => hr.StackIdFK == sr.IdPK))
        {
            lastStudied = _history.Where(hr => hr.StackIdFK == sr.IdPK).Max(hr => hr.StartedAt);
        }
        return new StackListItem()
        {
            Id = sr.IdPK,
            ViewName = sr.ViewName,
            Cards = _flashcards.Count(fr => fr.StackIdFK == sr.IdPK),
            LastStudied = lastStudied
        };
    }

    public Task<List<StackListItem>> GetStackListAsync(int? take = null, int skip = 0)
    {
        if (take == null)
        {
            var output = _stacks.Skip(skip).Select(sr => RowToStackListItem(sr)).ToList();
            return Task.FromResult(output);
        }
        else
        {
            var output = _stacks.Skip(skip).Take((int)take).Select(sr => RowToStackListItem(sr)).ToList();
            return Task.FromResult(output);
        }
    }

    public Task<StackListItem> GetStackListItemByIdAsync(int id)
    {
        var found = _stacks.Find(sr => sr.IdPK == id) ?? throw new ArgumentException($"No stack with ID {id} exists.");
        var output = RowToStackListItem(found);
        return Task.FromResult(output);
    }

    public Task<StackListItem?> GetStackListItemBySortNameAsync(string sortName)
    {
        StackListItem? output = null;
        var found = _stacks.Find(sr => sr.SortNameUQ == sortName);
        if (found == null)
        {
            return Task.FromResult(output);
        }
        output = RowToStackListItem(found);
        return Task.FromResult<StackListItem?>(output);
    }

    public Task<StackListItem> GetStackListItemByFlashcardIdAsync(int flashcardId)
    {
        var flashcard = _flashcards.Find(fr => fr.IdPK == flashcardId) ?? throw new ArgumentException($"No flashcard with ID {flashcardId} exists.");
        var found = _stacks.Find(sr => sr.IdPK == flashcard.StackIdFK) ?? throw new ApplicationException("Bad data: there's a flashcard with non-existant stack.");
        var output = RowToStackListItem(found);
        return Task.FromResult(output);
    }

    public Task CreateStackAsync(NewStack stack)
    {
        var newStack = new StacksRow(stack.SortName, stack.ViewName);
        _stacks.Add(newStack);
        return Task.CompletedTask;
    }

    public Task DeleteStackAsync(int stackId)
    {
        var found = _stacks.Find(sr => sr.IdPK == stackId) ?? throw new ArgumentException($"No stack with ID {stackId} exists.");
        var flashcardsToRemove = _flashcards.FindAll(fr => fr.StackIdFK == stackId);
        var historyRowsToRemove = _history.FindAll(hr => hr.StackIdFK == stackId);
        _historyToFlashcard.RemoveAll(h2f => historyRowsToRemove.Any(hr => hr.IdPK == h2f.HistoryIdFK) || flashcardsToRemove.Any(fr => fr.IdPK == h2f.FlashcardIdFK));
        _history.RemoveAll(hr => historyRowsToRemove.Any(h => h.IdPK == hr.IdPK));
        _flashcards.RemoveAll(fr => flashcardsToRemove.Any(f => f.IdPK == fr.IdPK));
        _stacks.Remove(found);
        return Task.CompletedTask;
    }

    public Task RenameStackAsync(int oldStackId, NewStack updatedStack)
    {
        var found = _stacks.Find(sr => sr.IdPK == oldStackId) ?? throw new ArgumentException($"No stack with ID {oldStackId} exists.");
        found.SortNameUQ = updatedStack.SortName;
        found.ViewName = updatedStack.ViewName;
        return Task.CompletedTask;
    }

    public Task<int> CountFlashcardsAsync(int stackId)
    {
        return Task.FromResult(_flashcards.Count(fr => fr.StackIdFK == stackId));
    }

    public Task<bool> CardInStack(int stackId, int flashcardId)
    {
        return Task.FromResult(_flashcards.Any(fr => fr.IdPK == flashcardId && fr.StackIdFK == stackId));
    }

    public Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId, int? take = null, int skip = 0)
    {
        if (take == null)
        {
            var output = _flashcards.Where(fr => fr.StackIdFK == stackId).Skip(skip).Select(fr => new ExistingFlashcard()
            {
                Id = fr.IdPK,
                Front = fr.Front,
                Back = fr.Back
            }).ToList();
            return Task.FromResult(output);
        }
        else
        {
            var output = _flashcards.Where(fr => fr.StackIdFK == stackId).Skip(skip).Take((int)take).Select(fr => new ExistingFlashcard()
            {
                Id = fr.IdPK,
                Front = fr.Front,
                Back = fr.Back
            }).ToList();
            return Task.FromResult(output);
        }
    }

    public Task<ExistingFlashcard> GetFlashcardByIdAsync(int id)
    {
        var found = _flashcards.Find(fr => fr.IdPK == id) ?? throw new ArgumentException($"No flashcard with ID {id} exists.");
        var output = new ExistingFlashcard()
        {
            Id = found.IdPK,
            Front = found.Front,
            Back = found.Back
        };
        return Task.FromResult(output);
    }

    public Task CreateFlashcardAsync(NewFlashcard flashcard)
    {
        var newRow = new FlashcardsRow(flashcard.StackId, flashcard.Front, flashcard.Back);
        _flashcards.Add(newRow);
        return Task.CompletedTask;
    }

    public Task UpdateFlashcardAsync(ExistingFlashcard flashcard)
    {
        var found = _flashcards.Find(fr => fr.IdPK == flashcard.Id);
        if (found != null)
        {
            found.Front = flashcard.Front;
            found.Back = flashcard.Back;
        }
        return Task.CompletedTask;
    }

    public Task MoveFlashcardAsync(int flashcardId, int newStackId)
    {
        // Care must be taken to keep the history correct. Thus, a new history row must be created with the same study date as the old one.
        // But first we check for any existing history rows for the new stack with the same date and time. If there are any, we use that one instead.
        IEnumerable<HistoryRow> oldHistoryRows = _history.Where(hr => _historyToFlashcard.Where(h2f => h2f.FlashcardIdFK == flashcardId).Select(h2f => h2f.HistoryIdFK).Distinct().Contains(hr.IdPK)).ToList();
        foreach (HistoryRow oldHistoryRow in oldHistoryRows)
        {
            var newHistoryRow = _history.Find(hr => hr.StackIdFK == newStackId && hr.StartedAt == oldHistoryRow.StartedAt) ?? new HistoryRow(newStackId, oldHistoryRow.StartedAt);
            List<HistoryToFlashcardRow> oldResultRows = _historyToFlashcard.Where(h2f => h2f.HistoryIdFK == oldHistoryRow.IdPK && h2f.FlashcardIdFK == flashcardId).ToList();
            foreach (HistoryToFlashcardRow oldResultRow in oldResultRows)
            {
                var newResultRow = new HistoryToFlashcardRow(newHistoryRow.IdPK, oldResultRow.FlashcardIdFK, oldResultRow.WasCorrect, oldResultRow.AnsweredAt);
                _historyToFlashcard.Add(newResultRow);
            }
            _historyToFlashcard.RemoveAll(h2f => oldResultRows.Contains(h2f));
            if (!_history.Contains(newHistoryRow))
            {
                _history.Add(newHistoryRow);
            }
            if (!_historyToFlashcard.Any(h2f => h2f.HistoryIdFK == oldHistoryRow.IdPK))
            {
                _history.Remove(oldHistoryRow);
            }
        }
        var found = _flashcards.Find(fr => fr.IdPK == flashcardId);
        if (found != null && _stacks.Any(sr => sr.IdPK == newStackId))
        {
            found.StackIdFK = newStackId;
        }
        return Task.CompletedTask;
    }

    public Task DeleteFlashcardAsync(int id)
    {
        var found = _flashcards.Find(fr => fr.IdPK == id) ?? throw new ArgumentException($"No flashcard with ID {id} exists.");
        _historyToFlashcard.RemoveAll(h2f => h2f.FlashcardIdFK == id);
        _flashcards.Remove(found);
        return Task.CompletedTask;
    }

    public Task<int> CountHistoryAsync()
    {
        return Task.FromResult(_history.Count);
    }

    public Task<List<HistoryListItem>> GetHistoryListAsync(int? take = null, int skip = 0)
    {
        var output = _history.ConvertAll(hr => new HistoryListItem()
        {
            Id = hr.IdPK,
            StartedAt = hr.StartedAt,
            StackViewName = _stacks.Find(sr => sr.IdPK == hr.StackIdFK)?.ViewName ?? throw new ApplicationException("Bad data: there's a history row with non-existant stack."),
            CardsStudied  = _historyToFlashcard.Count(h2f => h2f.HistoryIdFK == hr.IdPK),
            CorrectAnswers = _historyToFlashcard.Count(h2f => h2f.HistoryIdFK == hr.IdPK && h2f.WasCorrect)
        });
        if (take != null)
        {
            return Task.FromResult(output.Skip(skip).Take((int)take).OrderBy(r => r.StartedAt).ToList());
        }
        else
        {
            return Task.FromResult(output.Skip(skip).OrderBy(r => r.StartedAt).ToList());
        }
    }

    public Task AddHistoryAsync(NewHistory history)
    {
        var newRow = new HistoryRow(history.StackId, history.StartedAt);
        _history.Add(newRow);
        foreach (var result in history.Results)
        {
            var newResult = new HistoryToFlashcardRow(newRow.IdPK, result.FlashcardId, result.WasCorrect, result.AnsweredAt);
            _historyToFlashcard.Add(newResult);
        }
        return Task.CompletedTask;
    }

    public Task<List<ExistingStudyResult>> GetStudyResults(int historyId, int? take = null, int skip = 0)
    {
        List<HistoryToFlashcardRow> resultRows;

        if (take != null)
        {
            resultRows = _historyToFlashcard.Where(h2f => h2f.HistoryIdFK == historyId).OrderBy(h2f => h2f.AnsweredAt).Skip(skip).Take((int)take).ToList();
        }
        else
        {
            resultRows = _historyToFlashcard.Where(h2f => h2f.HistoryIdFK == historyId).OrderBy(h2f => h2f.AnsweredAt).Skip(skip).ToList();
        }

        return Task.FromResult(resultRows.ConvertAll(r => new ExistingStudyResult()
        {
            Ordinal = resultRows.IndexOf(r) + 1 + skip,
            Front = _flashcards.Find(f => f.IdPK == r.FlashcardIdFK)?.Front ?? throw new ApplicationException("Bad data: there's a study result row with non-existant flashcard."),
            WasCorrect = r.WasCorrect,
            AnsweredAt = r.AnsweredAt
        }));
    }
}
