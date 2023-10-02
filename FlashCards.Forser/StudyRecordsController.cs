namespace FlashCards.Forser
{
    internal class StudyRecordsController
    {
        private readonly DataLayer _dataLayer = new DataLayer();
        internal void ShowRecordsMenu()
        {
            MainMenuController mainMenuController = new MainMenuController();

            AnsiConsole.Clear();
            Menu.RenderTitle("Records Menu");
            int selectedRecordsMenu = AnsiConsole.Prompt(DrawMenu()).Id;

            switch (selectedRecordsMenu)
            {
                case 0:
                    ShowStudyRecords();
                    break;
                case 1:
                    mainMenuController.MainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
        private void ShowStudyRecords()
        {
            List<StudyRecords> studyRecords = _dataLayer.FetchAllStudyRecords();

            AnsiConsole.Clear();
            Menu.RenderTitle("Study Records");
            Table table = new Table();
            table.Expand = true;
            table.AddColumn("Year");

            if(studyRecords.Any())
            {
                foreach (var year in studyRecords)
                {
                    table.AddRow($"{year.SessionDate.Year}");
                }
                AnsiConsole.Write(table);
                int selectedYear = AnsiConsole.Ask<int>("Enter what year you want to show records from: ");
                ShowRecordsForYear(studyRecords, selectedYear);
            }
            else
            {
                AnsiConsole.WriteLine("No study records found!");
                AnsiConsole.WriteLine("Press any key to return to Study Records Menu");
                Console.ReadLine();
                ShowRecordsMenu();
            }
        }
        private void ShowRecordsForYear(List<StudyRecords> studyRecords, int selectedYear)
        {
            Table table = new Table();
            table.Expand();
            table.AddColumns("Date Studied", "Stack", "Score");

            foreach(StudyRecords record in studyRecords)
            {
                if(record.SessionDate.Year == selectedYear)
                {
                    table.AddRow($"{record.SessionDate.ToShortDateString()}", $"{record.StackName}", $"{record.Score}");
                }
            }
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("Press any key to return to stack menu");
            Console.ReadLine();
            ShowRecordsMenu();
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
