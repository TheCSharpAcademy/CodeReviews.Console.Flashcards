using Flashcards.TwilightSaw.Controller;
using Flashcards.TwilightSaw.Models;
using Spectre.Console;

namespace Flashcards.TwilightSaw.View;

internal class Menu
{
    public void MainMenu(AppDbContext context)
    {
        var cardStackController = new CardStackController(context);
        var flashcardController = new FlashcardController(context);
        var endSession = false;
        while (!endSession)
        {
            var input = UserInput.CreateChoosingList(["Manage Stacks",
                "Study", "View Study Session Data", "Exit"]);
            switch (input)
            {
                case "Manage Stacks":
                    new StackMenu(context, flashcardController, cardStackController).Menu();
                    break;
                case "Study":
                    new StudyMenu(context, flashcardController).Menu(cardStackController);
                    break;
                case "View Study Session Data":
                    var k = new StudyController(context).Read();
                    var table = new Table();
                    table.AddColumn("Date")
                        .AddColumn("Score")
                        .Centered();

                    foreach (var session in k)
                        table.AddRow(@$"{session.Date}",
                            $"{session.Score}");

                    AnsiConsole.Write(table);
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "Exit":
                    endSession = true;
                    break;
            }
        }
    }
}