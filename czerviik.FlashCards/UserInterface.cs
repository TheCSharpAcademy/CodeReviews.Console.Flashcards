using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace FlashCards;

public static class UserInterface
{
    public static Option OptionChoice { get; private set; }
    public static void MainMenu()
    {
        Header("main menu");

        string[] options = {
                "New Study Session",
                "New Flashcard",
                "Show Stacks",
                "Show Study Sessions",
                "Exit"};

        ChooseOptions(options);
    }

    public static void StudySession()
    {
        Header("new study session");

        UserInput.DisplayMessage("Under construction.");
    }

    public static void NewFlashcard(string[] stacks)
    {
        Header("new flashcard");

        if (stacks.Length != 0)
        {
            Console.WriteLine("Select a stack");

            var modifiedStacks = new string[stacks.Length + 2];
            stacks.CopyTo(modifiedStacks, 0);
            modifiedStacks[^2] = "Create a new stack";
            modifiedStacks[^1] = "Go back";

            ChooseOptions(modifiedStacks);
        }
    }

    public static void ShowStacks(string[] stacks)
    {
        Header("show stacks");

        if (stacks.Length != 0)
        {
            Console.WriteLine("Select a stack");

            var modifiedStacks = new string[stacks.Length + 2];
            stacks.CopyTo(modifiedStacks, 0);
            modifiedStacks[^2] = "Show all";
            modifiedStacks[^1] = "Go back";

            ChooseOptions(modifiedStacks);
        }
    }

    public static void ShowFlashcards(List<FlashcardReviewDto> flashcards, Stack stack)
    {
        var options = new Dictionary<string,Option> 
        {
                {"Update a Flashcard",Option.Option1},
                {"Delete a Flashcard",Option.Option2},
                {"Delete a Stack",Option.Option3},
                {"Go back", Option.Option4}
        };

        Header("show stacks");
        FlashcardsTable(flashcards, stack);
        ChooseOptions(options);
    }

    public static void ShowFlashcards(List<FlashcardReviewDto> flashcards, List<Stack> stacks)
    {
        string[] options = {
                "Update a Flashcard",
                "Delete a Flashcard",
                "Go back"};

        Header("show stacks");
        FlashcardsTable(flashcards, stacks);
        ChooseOptions(options);
    }

    public static void NewFlashcardQuestion(string currentStack)
    {
        Header($"new {currentStack} flashcard");

        Console.WriteLine("Enter a question (esc - Go back):");
    }

    public static void NewFlashcardAnswer(string currentStack, string question)
    {
        Header($"new {currentStack} flashcard");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine("Enter an answer (esc - Go back):");
    }

    public static void NewFlashcardConfirm(string currentStack, string question, string answer)
    {
        Header($"new {currentStack} flashcard");
        Console.WriteLine($"Stack: {currentStack}");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine($"Answer: {answer}");
        Console.WriteLine();
        ChooseOptions(["Confirm", "Enter again"]);
    }
    public static void AnotherFlashcard()
    {
        Console.Clear();
        Console.WriteLine("Flashcard saved!");
        Console.WriteLine();
        ChooseOptions(["Add another flashcard", "Done"]);
    }
    public static void NewStack()
    {
        Header("create new stack");
        Console.WriteLine("Enter stack's name: ");
    }

    public static void ShowStudySessions()
    {
        Header("show study sessions");

        UserInput.DisplayMessage("Under construction.");
    }

    private static void Header(string headerText)
    {
        Console.Clear();
        Console.WriteLine($"----- {headerText.ToUpper()} -----");
        Console.WriteLine();
    }

    private static void ChooseOptions(Dictionary<string, Option> options)
    {
       string selectedOptionText = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle("red")
            .AddChoices(options.Keys)
            );
        OptionChoice = options[selectedOptionText]; //dodělat i u dalších method, tedy převést na Option enum type
    }

    private static void FlashcardsTable(List<FlashcardReviewDto> flashcards, Stack stack)
    {
        var table = new Table()
        .AddColumns("Id", "Front side", "Back side")
        .Title(stack.Name)
        .Border(TableBorder.Rounded);
        foreach (var flashcard in flashcards)
        {
            if (flashcard.StackId == stack.Id)
                table.AddRow(flashcard.DisplayId.ToString(), flashcard.Question, flashcard.Answer);
        }
        AnsiConsole.Write(table);
        Console.WriteLine();
    }

    private static void FlashcardsTable(List<FlashcardReviewDto> flashcards, List<Stack> stacks)
    {
        foreach (var stack in stacks)
        {
            var table = new Table()
            .AddColumns("Id", "Front side", "Back side")
            .Title(stack.Name)
            .Border(TableBorder.Rounded);
            foreach (var flashcard in flashcards)
            {
                if (flashcard.StackId == stack.Id)
                    table.AddRow(flashcard.DisplayId.ToString(), flashcard.Question, flashcard.Answer);
            }
            AnsiConsole.Write(table);
            Console.WriteLine();
        }

    }
}


