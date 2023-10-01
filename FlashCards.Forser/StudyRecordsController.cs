namespace FlashCards.Forser
{
    internal class StudyRecordsController
    {
        internal void ShowRecordsMenu()
        {
            MainMenuController mainMenuController = new MainMenuController();

            AnsiConsole.Clear();
            Menu.RenderTitle("Records Menu");
            int selectedRecordsMenu = AnsiConsole.Prompt(DrawMenu()).Id;

            switch (selectedRecordsMenu)
            {
                case 0:
                    break;
                case 1:
                    mainMenuController.MainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
        private SelectionPrompt<Menu> DrawMenu()
        {
            SelectionPrompt<Menu> menu = new()
            {
                HighlightStyle = Menu.HighLightStyle
            };

            menu.Title("Select an [B]option[/]");
            menu.AddChoices(new List<Menu>()
            {
                new() { Id = 0, Text = "Show study records" },
                new() { Id = 1, Text = "Return to Main Menu"}
            });

            return menu;
        }
    }
}
