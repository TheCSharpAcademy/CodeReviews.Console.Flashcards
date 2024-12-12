using Spectre.Console;

namespace flashcard_app
{
    /// <summary>
    /// Responsible for showing the UI to the user
    /// </summary>
    class View
    {
        internal static void DisplayMainMenuOptions()
        {
            var table = new Table();
            table.AddColumn("Welcome to the flashcard app, make a selection: \n");
            table.AddRow("1. Manage Stacks");
            table.AddRow("2. Manage Flashcards");
            table.AddRow("3. Study");
            table.AddRow("4. View Study Session Data");

            table.AddRow("0. Exit");

            AnsiConsole.Write(table);
        }

        internal static void ShowStacksMainMenu(List<Stack> stacks)
        {
            Console.Clear();
            var table = new Table();
            table.AddColumn("Stack Name: \n");

            if (stacks.Count == 0)
            {
                table.AddRow($"<No stacks found in database.>");
            }
            else
            {
                foreach (var stack in stacks)
                {
                    table.AddRow($"{stack.Id}. {stack.Name}");
                }
            }

            AnsiConsole.Write(table);

            Console.WriteLine("");
        }

        internal static void ManageSingleStackMenu(string? stackName)
        {
            //Console.Clear();
            var table = new Table();
            table.AddColumn($"Stack: {stackName}: \n");
            table.AddRow($"1. Create a new flashcard for this stack.");
            table.AddRow($"2. View all flashcards in this stack.");
            table.AddRow($"0. Return to main menu.");
            AnsiConsole.Write(table);
        }

        internal static void ShowFlashcardMainMenu(List<FlashcardDTO> flashcards)
        {
            Console.Clear();
            DisplayFlashcardsHorizontally(flashcards);
            Console.WriteLine();

            var table = new Table();
            table.AddColumn($"Flashcard menu: \n");
            table.AddRow($"1. Edit an existing flashcard");
            table.AddRow($"2. Delete an existing flashcard");
            table.AddRow($"0. Return to main menu.");
            AnsiConsole.Write(table);
        }

        internal static void DisplayFlashcardsHorizontally(List<FlashcardDTO> flashcards)
        {
            if (flashcards.Count == 0 || flashcards == null)
            {
                Console.WriteLine("<No flashcards in database.>");
                return;
            }

            var grid = new Grid();

            foreach (var _ in flashcards)
            {
                grid.AddColumn(new GridColumn().Centered());
            }

            var tables = new List<Table>();
            int displayID = 1;   // The ID we will show the user allowing them to choose a flashcard

            foreach (var flashcard in flashcards)
            {
                var table = new Table()
                    .HideHeaders() // Disable headers
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("").Centered())
                    .AddColumn(new TableColumn("").Centered())
                    .AddRow("Flashcard ID", displayID.ToString())
                    .AddRow("Front", flashcard.FrontText)
                    .AddRow("Back", flashcard.BackText);

                tables.Add(table);
                displayID++;
            }

            // Add the tables horizontally in the grid
            grid.AddRow(tables.ToArray());

            AnsiConsole.Write(grid);
        }

        internal static void DisplaySingleFlashcard(FlashcardDTO flashcard)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[bold green]Front[/]").Centered())
                .AddRow(Markup.Escape(flashcard.FrontText));

            AnsiConsole.Write(table);
        }

        internal static void ShowStackManageMenu(int? stackID, List<FlashcardDTO> flashcards)
        {
            Console.Clear();

            View.DisplayFlashcardsHorizontally(flashcards);

            var table = new Table();
            table.AddColumn($"Current stack ID: {stackID}\n");
            table.AddRow($"1. Change current stack");
            table.AddRow($"2. Create a flashcard in stack");
            table.AddRow($"3. Edit a flashcard");
            table.AddRow($"4. Delete a flashcard");
            table.AddRow($"0. Return to main menu");

            AnsiConsole.Write(table);
        }

        internal static void ShowStudySessionData()
        {
            List<StudySession> studySessions = DBController.GetAllStudySessions();

            var table = new Table();
            table.Border = TableBorder.Rounded;
            table.AddColumn("Date");
            table.AddColumn("Score");
            table.AddColumn("Out of");
            table.AddColumn("Stack ID");

            // Add rows for each study session
            foreach (var session in studySessions)
            {
                table.AddRow(
                    session.SessionDate.ToString("yyyy-MM-dd HH:mm"),
                    session.Score.ToString(),
                    session.ScoreMax.ToString(),
                    session.StackID.ToString() + $" ({DBController.GetStackNameFromID(session.StackID)})"
                );
            }

            AnsiConsole.Write(table);
        }
    }
}