using Kolejarz.Flashcards.Domain.DAL;
using Kolejarz.Flashcards.Domain.DAL.Entities;
using Kolejarz.Flashcards.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace Kolejarz.Flashcards.Domain;

public class FlashardsService
{
    public FlashardsService()
    {
        using var context = new FlashcardsContext();
        context.Database.EnsureCreated();
    }

    public async Task Seed()
    {
        using var context = new FlashcardsContext();

        if (context.Stacks.Any()) return;

        var flashcards = new Dictionary<string, string>()
        {
            {"Abstract", "A keyword used to declare an abstract class or method, which cannot be instantiated and requires implementation in a derived class."},
            {"Class", "A blueprint for creating objects, which defines the properties and behavior of its instances."},
            {"Static", "A keyword used to declare a static member, which is shared among all instances of a class and can be called without creating an object."},
            {"Private", "A keyword used to declare a member that can only be accessed within the same class."},
            {"Virtual", "A keyword used to declare a virtual method, which can be overridden by a derived class."}
        };

        var stack = new FlashcardsStack
        {
            Name = "C# keywords",
            Description = "Learn most important C# keywords with this set of flashcards",
            CreatedDate = DateTime.Now,
            Flashcards = new List<Flashcard>()
        };
        await context.Stacks.AddAsync(stack);

        var cards = flashcards.Select(kvp => new Flashcard
        {
            Stack = stack,
            FrontSide = kvp.Key,
            BackSide = kvp.Value,
            CreatedDate = DateTime.Now
        });

        foreach(var card in cards)
        {
            stack.Flashcards.Add(card);
        }

        await context.SaveChangesAsync();
    }

    public IEnumerable<FlashcardsStackDto> GetAllStacks()
    {
        var result = new List<FlashcardsStackDto>();

        using var context = new FlashcardsContext();

        var stacks = context.Stacks.Include(s => s.Flashcards);
        foreach(var stack in stacks)
        {
            var flashcards = stack.Flashcards.Select(f => new FlashcardDto(f.FrontSide, f.BackSide));
            var stackDto = new FlashcardsStackDto(stack.Name, stack.Description, flashcards);
            result.Add(stackDto);
        }

        return result;
    }

    public async Task CreateNewStack(string name, string description)
    {
        using var context = new FlashcardsContext();

        await context.AddAsync(new FlashcardsStack
        {
            Name = name,
            Description = description,
            Flashcards = new(),
            CreatedDate = DateTime.Now
        });

        await context.SaveChangesAsync();
    }

    public async Task<FlashcardDto> CreateNewFlashcard(FlashcardsStackDto stackDto, string front, string back)
    {
        using var context = new FlashcardsContext();

        var stack = context.Stacks.Include(s => s.Flashcards).Single(s => s.Name == stackDto.Name);
        var flashcard = new Flashcard
        {
            FrontSide = front,
            BackSide = back,
            CreatedDate = DateTime.Now
        };

        stack.Flashcards.Add(flashcard);
        await context.SaveChangesAsync();
        return new FlashcardDto(front, back);
    }

    public async Task DeleteStack(FlashcardsStackDto stackDto)
    {
        using var context = new FlashcardsContext();

        var stack = await context.Stacks.SingleAsync(s => s.Name == stackDto.Name);
        context.Stacks.Remove(stack);

        await context.SaveChangesAsync();
    }

    public async Task<StudySessionDto> RecordStudySession(FlashcardsStackDto stackDto, int questionsAsked, int questionsAnswered)
    {
        using var context = new FlashcardsContext();

        var stack = context.Stacks.Include(s => s.Sessions).Single(s => s.Name == stackDto.Name);
        var session = new StudySession
        {
            CreatedDate = DateTime.Now,
            QuestionsAsked = questionsAsked,
            QuestionsAnswered = questionsAnswered
        };

        stack.Sessions.Add(session);
        await context.SaveChangesAsync();
        return new StudySessionDto(session.CreatedDate, stack.Name, questionsAsked / (float)questionsAnswered);
    }

    public async Task<IEnumerable<StudySessionDto>> GetAllStudySessions()
    {
        using var context = new FlashcardsContext();

        var sessions = await context.Sessions.Include(s => s.Stack).ToListAsync();
        return sessions.Select(s => new StudySessionDto(s.CreatedDate, s.Stack.Name, s.QuestionsAnswered / (float)s.QuestionsAsked));
    }
}
