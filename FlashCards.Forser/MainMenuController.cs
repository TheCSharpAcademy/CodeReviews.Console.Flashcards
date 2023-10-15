namespace FlashCards.Forser
{
    internal class MainMenuController
    {
        internal void MainMenu()
        {
            bool closeApp = false;

            while (closeApp == false)
            {
                StackController stackController = new StackController();
                StudyRecordsController studyRecordsController = new StudyRecordsController();
                StudyController studyController = new StudyController();

                AnsiConsole.Clear();
                Menu.RenderTitle("Main Menu");
                int selectedMenu = AnsiConsole.Prompt(DrawMenu()).Id;
                
                switch (selectedMenu)
                {
                    case 0:
                        studyController.ShowStudyMenu();
                        break;
                    case 1:
                        stackController.ShowStackMenu();
                        break;
                    case 2:
                        studyRecordsController.ShowRecordsMenu();
                        break;
                    case -1:
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        AnsiConsole.WriteLine("Not a valid option, select from an option from the Menu");
                        break;
                }
            }
        }
        private static SelectionPrompt<Menu> DrawMenu()
        {
            SelectionPrompt<Menu> menu = new()
            { 
                HighlightStyle = Menu.HighLightStyle
            };

            menu.Title("Select an [B]option[/]");
            menu.AddChoices(new List<Menu>()
            {
                new() { Id = 0, Text = "Study a Stack" },
                new() { Id = 1, Text = "Go to Stack Menu" },
                new() { Id = 2, Text = "View Study Records"},
                new() { Id = -1, Text = "Exit Application"}
            });

            return menu;
        }
    }
}