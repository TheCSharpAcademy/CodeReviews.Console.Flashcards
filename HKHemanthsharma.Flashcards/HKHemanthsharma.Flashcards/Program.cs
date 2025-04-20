using Spectre.Console;

namespace Flashcards.Study
{

    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseAccess dal = new DatabaseAccess();
            dal.Somemessage();
            DatabaseAccess.CreateDbIfNotExists();
            DatabaseAccess.CreateTables();
            bool end = false;
            while (!end)
            {
                Console.Clear();
                var table = new Table();
                table.Border = TableBorder.HeavyHead;
                TableTitle title = new TableTitle("project flashard");
                table.Title = title;
                table.AddColumn("select your choice:");
                table.AddRow("press 1 Manage the stacks");
                table.AddRow("press 2 Manage the flashcards");
                table.AddRow("press 3 to enter study area");
                table.AddRow("press 4 to View your study session report");
                table.AddRow("press 0 to stop the program");
                AnsiConsole.Write(table);
                int userChoice = UserInputs.Mainmenuselection();
                if (userChoice == 0) { end = true; }
                else
                {
                    switch (userChoice)
                    {
                        case 1:
                            UserOutputs.ManageStacks();
                            break;
                        case 2:
                            UserOutputs.ManageFlashcards();
                            break;
                        case 3:
                            UserOutputs.StudyArea();
                            break;
                        case 4:
                            UserOutputs.ViewStudySessionReport();
                            break;
                    }
                }
                Console.ReadLine();
            }
        }
    }
}