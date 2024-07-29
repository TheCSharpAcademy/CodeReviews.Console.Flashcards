
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
        var dtos = new List<FlashcardDTO>();
        var table = new Table();
        table.AddColumn("Question");
        table.AddColumn("Answer");

        foreach (var flashcard in stack.Flashcards) {
            var dto = new FlashcardDTO { Question = flashcard.Question, Answer = flashcard.Answer };
            dtos.Add(dto);
            table.AddRow(dto.Question, dto.Answer);
        }

        await Task.Delay(1); //Placeholder to make the function asynchronous

        AnsiConsole.Write(table);

        return true;
    }
}
