using Flashcards.DTOs;
using Spectre.Console;

namespace Flashcards.Handlers.Stack;
/// <summary>
/// Handles the viewing of flashcards in a stack.
/// </summary>
public class ViewFlashcardsHandler : IStackActionHandler {

    /// <summary>
    /// Handles the asynchronous operation of displaying flashcards from the specified stack.
    /// </summary>
    /// <param name="stack">The stack whose flashcards will be displayed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(Models.Stack stack) {
        // Convert flashcards to DTOs and sort them by question
        var dtoList = stack.Flashcards.Select(flashcard => new FlashcardDto {
            Question = flashcard.Question,
            Answer = flashcard.Answer
        }).ToList();
        dtoList.Sort((a, b) => string.Compare(a.Question, b.Question, StringComparison.OrdinalIgnoreCase));

        // Create and populate a table to display flashcards
        var table = new Table();
        table.AddColumn("Question");
        table.AddColumn("Answer");

        foreach (var dto in dtoList) {
            table.AddRow(dto.Question, dto.Answer);
        }

        await Task.Delay(1); // Placeholder to make the function asynchronous

        // Write the table to the console
        AnsiConsole.Write(table);
    }
}
