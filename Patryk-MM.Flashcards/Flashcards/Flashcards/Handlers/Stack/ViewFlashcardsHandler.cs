
using Flashcards.DTOs;
using Flashcards.Repositories;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
public class ViewFlashcardsHandler : IStackActionHandler {
    private readonly IStackRepository _stackRepository;

    public ViewFlashcardsHandler(IStackRepository stackRepository) {
        _stackRepository = stackRepository;
    }

    public async Task<bool> HandleAsync(Models.Stack stack) {
        var dtoList = stack.Flashcards.Select(flashcard => new FlashcardDTO { Question = flashcard.Question, Answer = flashcard.Answer }).ToList();
        dtoList.Sort((a, b) => string.Compare(a.Question, b.Question, StringComparison.OrdinalIgnoreCase));
        var table = new Table();
        table.AddColumn("Question");
        table.AddColumn("Answer");

        foreach (var dto in dtoList)
        {
            table.AddRow($"{dto.Question}", $"{dto.Answer}");
        }

        await Task.Delay(1); //Placeholder to make the function asynchronous
        AnsiConsole.Write(table);

        return true;
    }
}
