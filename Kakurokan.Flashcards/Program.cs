using Spectre.Console;
using System;
using System.Threading;

namespace Kakurokan.Flashcards
{
    internal class Program
    {
        public static void Main()
        {
            DataAcess dataAcess = new DataAcess();
            AnsiConsole.Clear();

            var input = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Welcome to your [red]Flashcard[/] app!")
        .AddChoices(new[] {"New Study Session",  "View Study Sessions", "Manage Stacks", "Exit"
        }));
            switch (input)
            {
                case "New Study Session":
                    StudySessionCreator.NewSession();
                    break;
                case "View Study Sessions":
                    dataAcess.ViewStudySessions();
                    break;
                case "Manage Stacks":
                    var new_input = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What do you want to do with your [Red]Stacks[/]?")
                    .AddChoices(new[] { "New Stack", "View stacks", "Delete a stack", "Return to menu" }));
                    switch (new_input)
                    {
                        case "New Stack":
                            CreateStack();
                            break;
                        case "View stacks":
                            dataAcess.ViewFlashcards(dataAcess.SelectStack());
                            break;
                        case "Delete a stack":
                            dataAcess.DeleteStack(dataAcess.SelectStack());
                            break;
                        case "Return to menu":

                            break;
                    }
                    DisplayReturningTomenu();
                    Main();
                    break;
                case "Exit":
                    Environment.Exit(0);
                    break;
            }
        }

        public static void CreateStack()
        {
            DataAcess dataAcess = new DataAcess();
            AnsiConsole.Write("What`s the name of your new stack? ");
            dataAcess.InsertStack(new Stacks(Console.ReadLine()));
        }

        public static void CreateFlashcard(int StackId)
        {
            DataAcess dataAcess = new DataAcess();
            AnsiConsole.Write("Add the question: ");
            string question = Console.ReadLine();
            AnsiConsole.Write("Add the answer: ");
            dataAcess.InsertFlashcard(new Flashcards(StackId, question, Console.ReadLine()));
        }

        public static void DisplayReturningTomenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Status().Start("Returning to menu...", ctx =>
        {
            Thread.Sleep(2000);
        });
            Main();

        }
    }
}