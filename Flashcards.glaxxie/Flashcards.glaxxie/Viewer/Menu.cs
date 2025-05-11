using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flashcards.glaxxie.Controllers;
using Flashcards.glaxxie.Enums;
using Microsoft.Data.SqlClient;
using Flashcards.glaxxie.Prompts;
using Spectre.Console;
using Microsoft.IdentityModel.Tokens;
using Flashcards.glaxxie.DTO;

namespace Flashcards.glaxxie.Display;

internal class Menu
{
    //private readonly SqlConnection _connection = DatabaseController.GetConnection();

    internal static void MainMenu()
    { 
        //Console.Clear();
        Headliner("MAIN MENU", "dim");
        var choice = General.SelectionInput<MainMenuOption>();
        Console.Clear();
        switch (choice)
        {
            case MainMenuOption.Practice:
                //SessionMenu();
                var pstack = Stack.Selection("Choose a stack to practice");
                break;
            case MainMenuOption.ReView:
                var rstack = Stack.Selection("Choose a stack to review");
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
                if (!newStack.Name.IsNullOrEmpty())
                    StackController.Insert(newStack);
                break;
            case ActionOption.Update:
                var uStack = Stack.UpdatePrompt();
                if (uStack != null)
                    StackController.Update(uStack);
                break;
            case ActionOption.Delete:
                var dStack = Stack.DeletePrompt();
                if (dStack >= 1)
                    StackController.Delete(dStack);
                break;
            default:
                Console.WriteLine("Back to main menu");
                back = Menus.Main;
                break;
        }
        // there is way to work the rule here to make it cleaner
        BackToMenu(back);
    }

    internal static void CardMenu()
    {
        Headliner("CARD MANAGEMENT", "dim");
        Menus back = Menus.Card;
        var choice = General.SelectionInput<ActionOption>();
        
        switch (choice)
        {
            case ActionOption.Add:
                //var newCard =  Card.InsertPrompt();
                //if (newCard != null)
                //    CardController.Insert(newCard);
                CardCreation? newCard;
                int counter = 0;
                do
                {
                    newCard = Card.InsertPrompt();
                    if (newCard != null)
                        CardController.Insert(newCard);
                        counter++;
                }
                while (newCard != null);
                break;
            case ActionOption.Update:
                var update = Card.UpdatePrompt();
                if (update != null)
                    CardController.Update(update);
                break;
            case ActionOption.Delete:
                var cardIds = Card.DeletePrompt();
                if (cardIds.Count() > 0)
                    CardController.Delete(cardIds);
                break;
            default:
                Console.WriteLine("Back to main menu");
                back = Menus.Main;
                break;
        }
        BackToMenu(back);
    }

    private static void SettingMenu()
    {
        Headliner("SETTINGS", "dim");
        Menus back = Menus.Setting;
        var choice = General.SelectionInput<Settings>();
        switch (choice)
        {
            case Settings.Report:
                //SessionMenu();
                //var pstack = Stack.Selection("Choose a stack to practice");
                break;
            case Settings.SeedData:
                //var rstack = Stack.Selection("Choose a stack to review");
                break;
            case Settings.WIPE:
                //StackMenu();
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

    internal static void ClearDisplay()
    {
        Console.WriteLine("press any key to continue");
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
