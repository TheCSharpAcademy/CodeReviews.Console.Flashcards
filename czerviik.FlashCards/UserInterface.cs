using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace FlashCards;

public static class UserInterface
{
    public static string? OptionChoice { get; private set; }
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

    public static void NewFlashcardQuestion(string currentStack)
    {
        Header($"new {currentStack} flashcard");

        Console.WriteLine("Enter a question:");
    }

     public static void NewFlashcardAnswer(string currentStack, string question)
    {
        Header($"new {currentStack} flashcard");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine("Enter an answer:");
    }

    public static void NewFlashcardConfirm(string currentStack, string question, string answer)
    {
        Header($"new {currentStack} flashcard");
        Console.WriteLine($"Stack: {currentStack}");
        Console.WriteLine($"Question: {question}");
        Console.WriteLine($"Answer: {answer}");
        Console.WriteLine("\nConfirm?");
        ChooseOptions(["Yes","No"]);
    }
    
    public static void NewStack()
    {
        Header("create new stack");
        Console.WriteLine("Enter stack's name: ");
    }
    public static void ShowStacks()
    {
        Header("show stacks");

        UserInput.DisplayMessage("Under construction.");
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

    private static void ChooseOptions(string[] options)
    {
        OptionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle("red")
            .AddChoices(options)
            );
    }
}


