using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Data;
public class DataAccess
{
    private readonly FlashcardsDbContext _context;

    public DataAccess(FlashcardsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Stack>> GetAllStacksAsync()
    {
        try
        {
            return await _context.Stacks.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Stack> GetStackByNameAsync(string stackName)
    {
        try
        {
            var stack = await _context.Stacks
                .Include(s => s.Flashcards)
                .Include(s => s.StudySessions)
                .FirstOrDefaultAsync(s => s.Name.ToLower() == stackName.ToLower());
            if (stack == null) return null;

            return stack;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<List<Stack>> GetAllStacksWithFlashcardsAsync()
    {
        try
        {
            return await _context.Stacks
            .Include(s => s.Flashcards)
            .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Stack> GetStackByIdAsync(int id)
    {
        try
        {
            var stack = await _context.Stacks.FirstOrDefaultAsync(s => s.Id == id);
            if (stack == null)
            {
                return null;
            }
            return stack;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Stack> GetStackWithFlashcardsByIdAsync(int id)
    {
        try
        {
            var stack = await _context.Stacks
            .Include(s => s.Flashcards)
            .FirstOrDefaultAsync(s => s.Id == id);
            if (stack == null)
            {
                return null;
            }

            return stack;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Stack> UpdateStackByIdAsync(int id, Stack Updatestack)
    {
        Stack stack = null;

        if (Updatestack == null) return null;
        if (Updatestack.Id != id) return null;

        try
        {
            stack = await _context.Stacks.FirstOrDefaultAsync(stack => stack.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        if (stack == null)
        {
            return null;
        }

        stack.Id = Updatestack.Id;
        stack.Name = Updatestack.Name;

        _context.Stacks.Update(stack);

        try
        {
            await _context.SaveChangesAsync();

            return stack;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Stack> DeleteStackByIdAsync(int id)
    {
        Stack stack = null;

        try
        {
            stack = await _context.Stacks.FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        if (stack == null) return null;

        _context.Stacks.Remove(stack);
        try
        {
            await _context.SaveChangesAsync();
            return stack;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Stack> AddStackAsync(Stack stack)
    {
        if (stack == null) return null;

        _context.Stacks.Add(stack);

        try
        {
            await _context.SaveChangesAsync();

            return stack;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Flashcard> GetFlashcardByIdAsync(int id)
    {
        try
        {
            var flashcard = await _context.Flashcards.FirstOrDefaultAsync(fc => fc.Id == id);
            if (flashcard == null) return null;

            return flashcard;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Flashcard> AddFlashcardAsync(int stackId, Flashcard flashcard)
    {
        Stack stack = null;

        if (flashcard == null) return null;

        try
        {
            stack = await _context.Stacks
                .Include(s => s.Flashcards)
                .FirstOrDefaultAsync(s => s.Id == stackId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        if (stack == null) return null;

        stack.Flashcards.Add(flashcard);

        try
        {
            await _context.SaveChangesAsync();
            return flashcard;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Flashcard> UpdateFlashcardAsync(int flashcardId, Flashcard updateFlashcard)
    {
        Flashcard flashcard = null;

        if (flashcardId != updateFlashcard.Id) return null;
        if (updateFlashcard == null) return null;

        try
        {
            flashcard = await _context.Flashcards.FirstOrDefaultAsync(fc => fc.Id == flashcardId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        if (flashcard == null) return null;

        flashcard.Question = updateFlashcard.Question;
        flashcard.Answer = updateFlashcard.Answer;

        _context.Flashcards.Update(flashcard);

        try
        {
            await _context.SaveChangesAsync();

            return flashcard;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Flashcard> DeleteFlashcardAsync(int stackId, int flashcardId)
    {
        Stack stack = null;

        try
        {
            stack = await _context.Stacks
                .Include(s => s.Flashcards)
                .FirstOrDefaultAsync(s => s.Id == stackId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        if (stack == null) return null;

        var flashcard = stack.Flashcards.FirstOrDefault(f => f.Id == flashcardId);
        if (flashcard == null) return null;

        stack.Flashcards.Remove(flashcard);

        try
        {
            await _context.SaveChangesAsync();
            return flashcard;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fail to delete record.Details: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Flashcard>> GetAllFlashcardsAsync()
    {
        try
        {
            return await _context.Flashcards.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Flashcard>> GetFlashcardsByStackNameAsync(string stackName)
    {
        try
        {
            return await _context.Flashcards
                .Where(fc => fc.Stack.Name.ToLower() == stackName.ToLower())
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<StudySession>> GetAllStudySessionsAsync()
    {
        try
        {
            return await _context.StudySessions
                .Include(ss => ss.Stack)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<StudySession> GetStudySessionByIdAsync(int studySessionId)
    {
        try
        {
            var studySession = await _context.StudySessions.FirstOrDefaultAsync(ss => ss.Id == studySessionId);

            if (studySession == null) return null;

            return studySession;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<StudySession>> GetStudySessionsByStackIdAsync(int stackId)
    {
        try
        {
            var studySessions = await _context.StudySessions
                .Where(ss => ss.StackId == stackId)
                .ToListAsync();

            if (studySessions == null) return null;

            return studySessions;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<StudySession> AddNewStudySessionAsync(StudySession studySession)
    {
        if (studySession == null) return null;

        try
        {
            _context.StudySessions.Add(studySession);

            await _context.SaveChangesAsync();

            return studySession;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fail to add record to database");
            throw new Exception(ex.Message);
        }
    }

    public void EnsureDbExists()
    {
        // Check if the database exists
        if (!_context.Database.CanConnect())
        {
            Console.WriteLine("Database does not exist. Creating...");

            // Create the database
            _context.Database.EnsureCreated();

            Console.WriteLine("Database created successfully.");
        }
        else
        {
            return;
        }
    }
}