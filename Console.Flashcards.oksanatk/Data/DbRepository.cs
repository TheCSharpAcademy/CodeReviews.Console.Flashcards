using dotnetMAUI.Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace dotnetMAUI.Flashcards.Data;

public class DbRepository 
{
    private readonly AppDbContext _context;

    public DbRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stack>> GetAllStacksAsync()
    {
        return await _context.Set<Stack>().ToListAsync();
    }

    public async Task<List<StudySession>> GetAllStudySessionsAsync()
    {
        return await _context.Set<StudySession>().ToListAsync();
    }

    public List<FlashcardDto> GetAllFlashcardsDisplay(int stackId)
    {
        return _context.Flashcards
            .Where(f => f.StackId == stackId)
            .Select(f => new FlashcardDto
            {
                Id = f.Id,
                Front = f.Front,
                Back = f.Back
            })
            .ToList();
    }

    public async Task<Stack> GetStackById(int stackId)
    {
        var foundStack = await _context.Stacks.FindAsync(stackId);
        if (foundStack != null)
        {
            return foundStack;
        }
        return new Stack();
    }

    public async Task CreateNewStack(string stackName)
    {
        bool stackExists = await _context.Stacks.AnyAsync(s => s.Name == stackName);
        if (stackExists)
        {
            return;
        }
        await _context.Stacks.AddAsync(new Stack { Name = stackName });
        await _context.SaveChangesAsync();
    }

    public async Task CreateNewFlashcard(int stackId, string front, string back)
    {
        await _context.Flashcards.AddAsync(new Flashcard { StackId = stackId, Front = front, Back = back });
        await _context.SaveChangesAsync();
    }

    public async Task CreateNewStudySession(DateTime dateStudied, int score, int stackId)
    {
        await _context.StudySessions.AddAsync(new StudySession { StackId = stackId, Score = score, DateStudied = dateStudied });
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFlashcardAsync(FlashcardDto flashcard)
    {
        var existingFlashcard = await _context.Flashcards.FindAsync(flashcard.Id);
        if (existingFlashcard != null)
        {
            existingFlashcard.Front = flashcard.Front;
            existingFlashcard.Back = flashcard.Back;
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFlashcardAsync(FlashcardDto flashcard)
    {
        var toBeDeleted = await _context.Flashcards.FindAsync(flashcard.Id);
        if (toBeDeleted != null)
        {
            _context.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteStack(Stack selectedStack)
    {
        var toBeDeleted = await _context.Stacks.FindAsync(selectedStack.Id);
        if (toBeDeleted != null)
        {
            _context.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<StudySessionPivotDto>> GetPivotedStudySessionsAsync(int year)
    {
        var studySessions = await _context.StudySessions
            .Where(s => s.DateStudied.Year == year)
            .GroupBy(s => new { s.StackId, s.Stack.Name, s.DateStudied.Month })
            .Select(g => new
            {
                g.Key.Name,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .ToListAsync();

        var pivotedTable = studySessions
            .GroupBy(s => s.Name)
            .Select(g => new StudySessionPivotDto
            {
                StackName = g.Key,
                MonthlyCounts = g.ToDictionary(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month), x => x.Count)
            })
            .ToList();

        return pivotedTable;
    }
}
