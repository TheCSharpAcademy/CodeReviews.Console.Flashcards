using Flashcards.ConsoleApp.Services;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to study a stack of flashcards and return a score.
/// </summary>
internal class StudyStackPage : BasePage
{
    #region Constants

    private const string PageTitle = "Study Stack";

    #endregion
    #region Methods

    internal static int Show(StackDto stack, IReadOnlyList<FlashcardDto> flashcards)
    {
        int score = 0;

        for (int i = 0; i < flashcards.Count; i++)
        {
            AnsiConsole.Clear();
            
            WriteHeader($"{PageTitle}: {stack.Name}");

            AnsiConsole.WriteLine($"Question {i + 1} of {flashcards.Count}:");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[olive]{flashcards[i].Question}[/]");
            AnsiConsole.WriteLine();

            var answer = UserInputService.GetString($"Enter the [blue]answer[/] to this flashcard, or [blue]0[/] to return to stop studying: ");
            if (answer == "0")
            {
                break;
            }

            AnsiConsole.WriteLine();

            if (string.Equals(flashcards[i].Answer, answer, StringComparison.OrdinalIgnoreCase))
            {
                score++;
                AnsiConsole.MarkupLine("[green]Correct[/]!");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Incorrect[/]! The correct answer is [green]{flashcards[i].Answer}[/]");
            }

            WriteFooter();

            // Await user confirmation to continue.
            Console.ReadKey();
        }

        return score;
    }

    #endregion
}
