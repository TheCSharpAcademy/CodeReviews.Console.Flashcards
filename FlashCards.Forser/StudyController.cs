namespace FlashCards.Forser
{
    internal class StudyController
    {
        internal void ShowStudyMenu()
        {
            MainMenuController mainMenuController = new MainMenuController();

            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("              STUDY MENU");
            Console.WriteLine("Select - Select a Stack to Study");
            Console.WriteLine("Menu - Return to Main Menu");
            Console.WriteLine("------------------------------------------");

            string selectedStudyMenu = Console.ReadLine().Trim().ToLower();

            switch (selectedStudyMenu)
            {
                case "menu":
                    mainMenuController.MainMenu();
                    break;
                case "select":
                    break;
            }
        }
    }
}