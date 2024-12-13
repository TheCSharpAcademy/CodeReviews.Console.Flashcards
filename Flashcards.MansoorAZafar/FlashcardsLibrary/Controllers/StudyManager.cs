using FlashcardsLibrary.Models;
using FlashcardsLibrary.Views;
using Spectre.Console;

namespace FlashcardsLibrary.Controllers;

internal class StudyManager
{
    private bool recordData;
    internal StudyManager(bool recordData = false)
    {
        this.recordData = recordData;
    }

    internal void Study()
    {
        DataViewer.DisplayHeader("Study");
        if(Utilities.EmptyStack != false)
        {
            System.Console.WriteLine("There are no stacks, please create one or many and come back");
            return;
        }
        
        System.Console.WriteLine($"Current Stack: {Utilities.CurrentStack}\n");
        System.Console.WriteLine("To select/change a stack follow these steps (The latest stack modified in Manage Stacks will be selected)\n 1. Go to the Home Menu\n 2. Select \"Manage Stacks\"\n 3. Enter the ID of the stack you want to enter\n 4. Come back here\n");
        
        if(string.IsNullOrEmpty(Utilities.CurrentStack)) return;
        Utilities.PressToContinue();
        
        this.PracticeSession();
    }

    private void PracticeSession()
    {
        int score = 0;
        System.Console.Clear();
        List<Flashcard> flashcards 
            = Utilities.databaseManager.GetAllDataWithTopic<Flashcard>(Utilities.FlashcardTableName, Utilities.CurrentStack);

        foreach(var flashcard in flashcards)
        {
            DataViewer.DisplayHeader("Study");
            var panel = new Panel(flashcard.FrontOfTheCard ?? "n/a")
                                 .Header("Front")
                                 .SquareBorder()
                                 .Collapse()
                                 .BorderColor(Color.LightSkyBlue1)
                                 .Padding(4,2,4,2);
            AnsiConsole.Write(panel);

            string input = Utilities.GetStringInput(message: "\nInput your answer to this card\nOr 0 to exit\n> ").ToLower();
            if("0".Equals(input))
                break;

            if(input == flashcard.BackOfTheCard?.ToLower())
                System.Console.WriteLine($"\nCorrect! The answer is indeed {input}\n Your score is {++score}");
            else 
                System.Console.WriteLine($"\nYour answer was wrong.\n\nYou answered {input}\nThe correct answer was {flashcard.BackOfTheCard}");
            
            Utilities.PressToContinue();
            System.Console.Clear();
        }
        DataViewer.DisplayHeader("Study Result");
        System.Console.WriteLine($"Exiting Study Session\nYou got {score} right out of {flashcards.Count}");
        
        if(this.recordData)
        {
            DateTime dateTime = DateTime.Now;
            Utilities.databaseManager
                .InsertSession(score, dateTime, Utilities.CurrentStack);
        }
    }
}
