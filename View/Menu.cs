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
                    var endManage = false;
                    while (!endManage)
                    {
                        var inputStacks = UserInput.CreateChoosingList(["Choose a Stack", "Create a new Stack", "Return"]);
                        switch (inputStacks)
                        {
                            case "Create a new Stack":
                                var stackName = UserInput.Create("Name your new Stack: ");
                                cardStackController.Create(stackName);
                                Console.Clear();
                                break;
                            case "Choose a Stack": 
                                var chosenStack = UserInput.ChooseStack(cardStackController.Read());
                                var endStack = false;
                                while (!endStack)
                                {
                                    var inputCreateStacks = UserInput.CreateChoosingList([
                                        "Create a Flashcard in the Stack",
                                        "View all Flashcards in the Stack", "Edit a Flashcard in the Stack", "Delete a Flashcard in the Stack",
                                        "Return"
                                    ]);
                                    switch (inputCreateStacks)
                                    {
                                        case "Create a Flashcard in the Stack":
                                            //REGEX? 
                                            var frontSideInput = UserInput.Create("Name the front side of your Flashcard: ");
                                            var backSideInput = UserInput.Create("Name the back side of your Flashcard: ");
                                            flashcardController.Create(frontSideInput, backSideInput, chosenStack.CardStackId);
                                            break;
                                        case "View all Flashcards in the Stack":
                                            var endRead = true;
                                            while (endRead)
                                            {
                                               var read = flashcardController.Read(chosenStack
                                                             .CardStackId);
                                               AnsiConsole.Write(UserInput.CreateFlashcardTable(read));

                                                var inputView = UserInput.CreateChoosingList(["Return"]);
                                                switch (inputView)
                                                {
                                                    case "Return":
                                                        endRead = false;
                                                        break;
                                                }
                                                Console.Clear();
                                            }
                                            break;
                                        case "Edit a Flashcard in the Stack":
                                            //Late validation
                                            var inputUpdate = flashcardController.GetFlashcard(flashcardController, chosenStack);
                                            var inputUpdateSide = UserInput.CreateChoosingList(["Front side", "Back side"]);
                                            var newFlashcardSide = UserInput.Create(inputUpdateSide == "Front side" ? "Type your new Front Side" : "Type your new Back Side");
                                            var executeUpdate = Validation.Validate(() => flashcardController.Update(inputUpdate.Id, chosenStack.CardStackId, (inputUpdateSide,newFlashcardSide)));
                                            //Duplicate code
                                            AnsiConsole.Markup($"[olive]{executeUpdate}[/]");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        case "Delete a Flashcard in the Stack":
                                            var inputDelete = flashcardController.GetFlashcard(flashcardController, chosenStack);
                                            var inputView1 = UserInput.CreateChoosingList(["Return"]);
                                            switch (inputView1)
                                            {
                                                case "Return":
                                                    endRead = false;
                                                    break;
                                            }
                                            var executeDelete = Validation.Validate(() => flashcardController.Delete(inputDelete.Id, chosenStack.CardStackId));
                                            AnsiConsole.Markup($"[olive]{executeDelete}[/]");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        case "Return":
                                            endStack = true;
                                            break;
                                    }
                                }
                                break;
                            case "Return":
                                endManage = true;
                                break;
                        }
                    }
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