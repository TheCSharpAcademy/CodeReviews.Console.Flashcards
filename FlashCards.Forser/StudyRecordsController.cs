namespace FlashCards.Forser
{
    internal class StudyRecordsController
    {
        internal void ShowRecordsMenu()
        {
            MainMenuController mainMenuController = new MainMenuController();

            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("              RECORDS MENU");
            Console.WriteLine("Show - Show study records");
            Console.WriteLine("Menu - Return to Main Menu");
            Console.WriteLine("------------------------------------------");

            string selectedRecordsMenu = Console.ReadLine().Trim().ToLower();

            switch (selectedRecordsMenu)
            {
                case "menu":
                    mainMenuController.MainMenu();
                    break;
                case "show":
                    break;
                default:
                    Console.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
    }
}
