using FlashcardsLibrary.Models;
using FlashcardsLibrary.Controllers;
using FlashcardsLibrary.Views;

namespace FlashcardsLibrary;

public class FlashcardProgramManager
{
    private readonly HomeMenuController HomeMenuHandler = new();
    public void Begin()
    {
        //By default it's exit but because of do-while it'll run atleast once
        HomeMenu homeSelection = HomeMenu.exit;
        do
        {
            System.Console.Clear();
            
            DataViewer.Figlet("Flashcards", "center");
            DataViewer.DisplayHeader("Home");

            homeSelection = 
                Utilities.GetSelection<HomeMenu>
                (
                    enumerationValues: Enum.GetValues<HomeMenu>(),
                    title: "[green]Select Your Option[/]",
                    alternateNames: item => item switch 
                    {
                        HomeMenu.FlashCardManager => "Manage FlashCards",
                        HomeMenu.StackManager => "Manage Stacks",
                        HomeMenu.StudySession => "Study Session",
                        _ => item.ToString()
                });
            
            this.HomeMenuHandler.HandleHomeMenuSelection(homeSelection);
            Utilities.PressToContinue();

        } while(homeSelection != HomeMenu.exit);
        
        System.Console.Clear();
        System.Console.WriteLine();
        DataViewer.Figlet("Goodbye");
    }
}
