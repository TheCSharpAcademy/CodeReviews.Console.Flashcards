using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.Enums;
using Flashcards.glaxxie.Prompts;
using Spectre.Console;
using Flashcards.glaxxie.DTO;
using Flashcards.glaxxie.Viewer;
using Flashcards.glaxxie.DataSeeder;

namespace Flashcards.glaxxie.Display;

internal class Menu
{
    internal static void MainMenu()
    { 
        Headliner("MAIN MENU", "dim");
        var choice = General.SelectionInput<MainMenuOption>();
        Console.Clear();
        switch (choice)
        {
            case MainMenuOption.Practice:
                StackViewer? pstack;
                do
                {
                    Console.Clear();
                    pstack = Stack.Selection("Choose a stack to practice");
                    if (pstack != null)
                        PracticeUI.Practice(pstack);
                } while (pstack != null);
                break;
            case MainMenuOption.ReView:
                StackViewer? rstack;
                do
                {
                    Console.Clear();
                    rstack = Stack.Selection("Choose a stack to review");
                    if (rstack != null)
                        ReviewUI.Review(rstack);

                } while (rstack != null);
                break;
            case MainMenuOption.ManageStacks:
                StackMenu();
                break;
            case MainMenuOption.ManageCards:
                CardMenu();
                break;
            case MainMenuOption.Settings:
                SettingMenu();
                break;
            default:
                Console.WriteLine("Exiting the program");
                Environment.Exit(0);
                break;
        }
    }

    internal static void StackMenu()
    {
        Headliner("STACK MANAGEMENT", "dim");
        Menus back = Menus.Stack;
        var choice = General.SelectionInput<ActionOption>();
        switch (choice)
        {
            case ActionOption.Add:
                var newStack = Stack.InsertPrompt();
                if (newStack != null)
                    StackController.Insert(newStack);
                break;
            case ActionOption.Update:
                var uStack = Stack.UpdatePrompt();
                if (uStack != null)
                    StackController.Update(uStack);
                break;
            case ActionOption.Delete:
                var stackId = Stack.DeletePrompt();
                if (stackId >= 1)
                    StackController.Delete(stackId);
                break;
            default:
                Console.WriteLine("Back to main menu");
                back = Menus.Main;
                break;
        }
        BackToMenu(back);
    }

    internal static void CardMenu()
    {
        Headliner("CARD MANAGEMENT", "dim");
        var choice = General.SelectionInput<ActionOption>();
        if (choice == ActionOption.Back)
        {
            BackToMenu(Menus.Main);
        }

        var stack = Stack.Selection("Choose a stack");
        if (stack != null)
        {
            switch (choice)
            {
                case ActionOption.Add:
                    Console.WriteLine("Leave either side blank to cancel\n");
                    CardCreation? newCard;
                    int counter = 0;
                    do
                    {
                        newCard = Card.InsertPrompt(stack);
                        if (newCard != null)
                        {
                            CardController.Insert(newCard);
                            Console.WriteLine();
                            counter++;
                        }
                        AnsiConsole.MarkupLine($" * Added [bold]{counter}[/] new {(counter == 1 ? "card" : "cards")} to {stack.Name}\n");
                    }
                    while (newCard != null);
                    break;
                case ActionOption.Update:
                    var update = Card.UpdatePrompt(stack);
                    if (update != null)
                        CardController.Update(update);
                    break;
                case ActionOption.Delete:
                    var cardIds = Card.DeletePrompt(stack);
                    if (cardIds.Any())
                        CardController.Delete(cardIds);
                    break;
            }
        }
        BackToMenu(Menus.Card);
    }

    private static void SettingMenu()
    {
        Headliner("SETTINGS", "dim");
        Menus back = Menus.Setting;
        var choice = General.SelectionInput<Settings>();
        switch (choice)
        {
            case Settings.Report:
                var year = General.SelectionInputInt("Pick a year", SessionController.GetYears());
                ReportUI.Display(year);
                break;
            case Settings.Data:
                Seeder.GenerateData();
                break;
            case Settings.Styles:
                Console.WriteLine("THIS IS WIP. COME BACK LATER");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Back to main menu");
                back = Menus.Main;
                break;
        }
        BackToMenu(back);
    }

    internal static void BackToMenu(Menus menu)
    {
        Console.SetCursorPosition(0, 1);
        Console.Clear();
        switch (menu)
        {
            case Menus.Main:
                MainMenu();
                break;
            case Menus.Card:
                CardMenu();
                break;
            case Menus.Stack:
                StackMenu();
                break;
            case Menus.Setting:
                SettingMenu();
                break;
            default:
                Console.WriteLine("Exiting the program");
                Environment.Exit(0);
                break;
        }
    }

    internal static void ClearDisplay(string msg = "Press any key to continue...")
    {
        AnsiConsole.MarkupLine($"[dim]{msg}[/]");
        Console.ReadKey();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
    }

    internal static void Headliner(string title, string style, string color = "red")
    {
        Rule rule = new($"[{color}]{title}[/]")
        {
            Style = Style.Parse($"{style}")
        };
        rule.LeftJustified();
        AnsiConsole.Write(rule);
    }
}