using FlashCards.Data;
using Spectre.Console;

namespace FlashCards.Utilities;

public static class StudyExtensions
{
    internal static string AskUserForAnswer()
    {
        string answer = AnsiConsole.Prompt(
            new TextPrompt<string>("Input your [cyan]answer[/] to this card or press [cyan]'0'[/] to exit: ")
                .ValidationErrorMessage("[red]Invalid input![/]")
                .Validate(input =>
                {
                    return input == "0" || !string.IsNullOrEmpty(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Answer cannot be empty![/]");
                }));
        return answer;
    }

    internal static int CheckCorrectAnswer(string answer, FlashCardDto flashcard)
    {
        answer = answer.ToLower();
        string correctAnswer = flashcard.back.ToLower();
        if (answer == "0") return -1; // Exit signal
        
        if (answer == correctAnswer)
        {
            AnsiConsole.MarkupLine("[green]Your answer is correct![/]");
            Util.AskUserToContinue();
            return 1; // Correct answer
        }

        AnsiConsole.MarkupLine("Your answer was wrong.");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"The correct answer was {correctAnswer}.");
        Util.AskUserToContinue();
        return 0; // Incorrect answer
    }

    internal static void ExitSessionReport(int score, int flashcardAmount)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Exiting Study session");
        AnsiConsole.MarkupLine($"You got {score} right out of {flashcardAmount}");
        Util.AskUserToContinue();
    }
}