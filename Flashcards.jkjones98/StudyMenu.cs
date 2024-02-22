using Flashcards.Functions;

namespace Flashcards.jkjones98;

internal class StudyMenu
{
    internal void DisplayStudyMenu()
    {
        MainMenu mainMenu = new();
        ViewStudySessions viewStudySessions = new();
        StartStudySession startStudySession = new();
        CheckUserInput stackChoice = new ();
        int stackId = stackChoice.ChooseStack();
        Console.WriteLine("\n\nHow many flashcards would you like to study in this session?");
        Console.WriteLine("\nEnter 1 for 5 Cards");
        Console.WriteLine("Enter 2 for 10 Cards");
        Console.WriteLine("Enter 3 for 15 Cards");
        Console.WriteLine("Enter 4 for 20 Cards");
        Console.WriteLine("Enter 0 to return to the main menu");
        string studyInput = Console.ReadLine();
        int sessionNum;
        while(!Int32.TryParse(studyInput, out sessionNum))
        {
            Console.WriteLine("\n\nPlease re-enter your menu selection");
            studyInput = Console.ReadLine();
        }

        switch(sessionNum)
        {
            case 1:
                startStudySession.StartSession(stackId, 5);
                break;
            case 2:
                startStudySession.StartSession(stackId, 10);
                break;
            case 3:
                startStudySession.StartSession(stackId, 15);
                break;
            case 4:
                startStudySession.StartSession(stackId, 20);
                break;
            case 5: 
                viewStudySessions.ViewSessionMenu();
                break;
            case 0:
                mainMenu.DisplayMenu();
                break;
        }

    }
}