using Flashcards.TwilightSaw.Controller;
using Spectre.Console;

namespace Flashcards.TwilightSaw.View;

internal class Menu
{
    public void MainMenu(AppDbContext context)
    {
        var cardStackController = new CardStackController(context);
        var flashcardController = new FlashcardController(context);
        var studyController = new StudyController(context);
        var endSession = false;
        while (!endSession)
        {
            var input = UserInput.CreateChoosingList(["Manage Stacks",
                "Study", "View Study Session Data", "View Study Session Report", "Exit"]);
            switch (input)
            {
                case "Manage Stacks":
                    new StackMenu(context, flashcardController, cardStackController).Menu();
                    break;
                case "Study":
                    new StudyMenu(context, flashcardController).Menu(cardStackController);
                    break;
                case "View Study Session Data":
                    var sessionsList = new StudyController(context).Read();
                    var table = new Table();
                    table.AddColumns(["Date", "Score", "Card Stack Name"])
                        .Centered();

                    foreach (var session in sessionsList)
                        table.AddRow(@$"{session.Date}",
                            $"{session.Score}",
                            $"{session.Name}");

                    AnsiConsole.Write(table);
                   Validation.EndMessage("");
                    break;
                case "View Study Session Report":
                    var date = UserInput.CreateRegex(@"^(\d{4})$", "Insert desired year: ", "Wrong symbols.");
                    studyController.GetTable("COUNT", date ,"Number of sessions per month");
                    studyController.GetTable("AVG", date,"Avg sessions score per month");
                    Validation.EndMessage("");
                    break;
                case "Exit":
                    endSession = true;
                    break;
            }
        }
    }
}