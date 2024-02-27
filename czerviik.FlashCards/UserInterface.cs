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

    public static void NewFlashCard()
    {
        Header("new flash card");

        UserInput.DisplayMessage("Under construction.");
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
        Console.WriteLine($"-----{headerText.ToUpper()}-----");
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


