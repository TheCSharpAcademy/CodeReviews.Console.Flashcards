using FlashcardGame.Helpers;

namespace FlashcardGame.Views
{
    internal class StudyMenu
    {
        public static void RunStudyMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.Clear();
                Console.WriteLine("What you want to do?");
                Console.WriteLine("a. View study sessions");
                Console.WriteLine("b. Start studying session");
                Console.WriteLine("c. Return to main menu");

                var option = Console.ReadLine();

                switch (option)
                {
                    case "a":
                        DatabaseHelpers.ViewStudySessions();
                        break;
                    case "b":
                        StudyGame.PickStackMenu();
                        break;
                    case "c":
                        MainMenu.Get1UserInput();
                        break;
                    default:
                        Console.WriteLine("Wrong option. Try again!");
                        Console.WriteLine("Press any key to continue: ");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
