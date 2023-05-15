using ConsoleTableExt;
using Flashcards.CRUD;
using Flashcards.Helpers;
using Flashcards.Model;

namespace Flashcards.Menus {
    internal class ManageStudySessionMenu {

        internal static void ManageStudySessions( Stack stack ) {

            bool managing = true;


            while (managing) {

                Console.Clear();

                Console.WriteLine(@"What would you like to do?
1 - Start new study session
2 - See past study sessions
3 - See detailed information
0 - Go back to the main menu");

                string op = Console.ReadLine();

                switch (op) {
                    case "0":
                        Console.Clear();
                        managing = false;
                        break;
                    case "1":
                        StudySessionMenu.StudySession(stack);
                        break;
                    case "2":
                        ShowPastSessions(stack);
                        break;
                    case "3":
                        GetAdvancedReport(stack);
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
            MainMenu.GetMainInput();
        }

        static void ShowPastSessions( Stack stack ) {

            bool viewing = true;

            while (viewing) {

                Console.Clear();
                List<StudySessionDto> sessions = DbOperations.GetStudySessions(stack);

                ConsoleTableBuilder.From(sessions)
                    .ExportAndWriteLine();

                Console.WriteLine("Press 0 to go back");

                string op = Console.ReadLine();

                if (op == "0") {
                    Console.Clear();
                    viewing = false;
                } else {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        static void GetAdvancedReport( Stack stack ) {

            bool viewing = true;

            while (viewing) {

                Console.Clear();
                Console.WriteLine("Please inform the year you want to filter");
                string year = Console.ReadLine();
                Console.Clear();

                List<List<Object>> sessionNumber = DbOperations.ChallengeReport(stack, year, true);

                ConsoleTableBuilder
                .From(sessionNumber)
                .WithTitle($"Number of sessions in {year}")
                .WithColumn("Stack Name", "January", "February", "March", "April", "May", "June", "July", "August", "Septmber", "October", "November", "December")
                .ExportAndWriteLine();

                HelpersAndValidation.InsertSeparator();

                List<List<Object>> sessionAverage = DbOperations.ChallengeReport(stack, year, false);

                ConsoleTableBuilder
                .From(sessionAverage)
                .WithTitle($"Average scores during {year}")
                .WithColumn("Stack Name", "January", "February", "March", "April", "May", "June", "July", "August", "Septmber", "October", "November", "December")
                .ExportAndWriteLine();

                Console.WriteLine("Press 0 to go back");

                string op = Console.ReadLine();

                if (op == "0") {
                    Console.Clear();
                    viewing = false;
                } else {
                    Console.WriteLine("Invalid input");
                }
            }
        }
    }
}
