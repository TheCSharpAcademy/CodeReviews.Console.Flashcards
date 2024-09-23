using FlashcardsLibrary.Models;
using Spectre.Console;

namespace FlashcardsLibrary;

public class Validator
{
    public static string AskForNewName(string input)
    {
        return AnsiConsole.Prompt(new TextPrompt<string>($"Please provide the {input}:"));
    }

    public static int GetStackIdFromName(string stackName)
    {
        var stack = (Stacks.StackList.Find(s => s.StackName == stackName));
        if (stack is null)
            throw new Exception(stackName);
        else return stack.StackId;
    }

    public static string GetStackNameFromId(int stackId)
    {
        var stack = Stacks.StackList.Find(s => s.StackId == stackId);
        if (stack is null)
            throw new Exception();
        else return stack.StackName;
    }

    public static string ValidateString(string input = "")
    {
        while (string.IsNullOrWhiteSpace(input))
        {
            input = AnsiConsole.Prompt(new TextPrompt<string>("[red]Invalid input. Try again:[/]"));
        }
        return input.ToLower().Trim();
    }

    public static string ValidateStackName()
    {
        IEnumerable<string> choices = Stacks.StackList.Select(s => s.StackName);
        return AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Choose a stack:")
            .AddChoices(choices));
    }

    public static int ValidateFlashcardChoice(int stackId)
    {
        List<Flashcards> filtered = Flashcards.FlashcardsList.Where(f => f.StackId == stackId).ToList();
        Dictionary<string, int> choices = [];
        for (int i = 0; i < filtered.Count(); i++)
        {
            choices.Add($"Flashcard Id: {i + 1}, Front: {filtered[i].Front}, Back: {filtered[i].Back}", filtered[i].FlashcardId);
        }
        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Choose a flashcard:")
            .AddChoices(choices.Keys));

        return choices[choice];
    }

    public static int ValidateYearChoiceForReport()
    {
        List<int> input = GetAvailableYearsForReports();
        return AnsiConsole.Prompt(new SelectionPrompt<int>().
            Title("Choose a year for the report:")
            .AddChoices(input));
    }

    private static List<int> GetAvailableYearsForReports()
    {
        return StudySessions.StudySessionsList.Select(s => s.Date.Year)
                                              .Distinct()
                                              .ToList();
    }
}

