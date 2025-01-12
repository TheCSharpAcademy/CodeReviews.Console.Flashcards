using FlashCards.Control;

namespace FlashCards.View
{
    internal static class StudySessionMenu
    {
        internal static void DisplayStudySessionMenu()
        {
            while (true)
            {
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("STUDY SESSION MENU");
                Console.WriteLine("--------------------------------------------------------------------------------");
                Console.WriteLine("1. Start Study Session");
                Console.WriteLine("2. View Study Session History");
                Console.WriteLine("3. Return to main menu\n");
                Console.WriteLine("Choose an option from the menu.");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        StudySessionControl.StartStudySession();
                        break;
                    case "2":
                        Console.Clear();
                        StudySessionControl.ViewStudySessions();
                        break;
                    case "3":
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid input. Please try again.\n");
                        break;
                }
            }
        }
    }
}
