using Flashcards.TwilightSaw.Controller;
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
                                        "View all Flashcards in the Stack", "Delete a Flashcard in the Stack",
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
                                                
                                                var table = new Table();
                                                table.AddColumn("Number")
                                                    .AddColumn("Front")
                                                    .AddColumn("Back")
                                                    .Centered();
                                                var read = flashcardController.Read(chosenStack
                                                             .CardStackId);
                                                for (var index = 0; index < read.Count; index++)
                                                {
                                                    var flashcard = read[index];
                                                    table.AddRow(@$"{index+1}",
                                                        $"{FlashcardMapper.ConvertToDto(flashcard).Front}",
                                                        $"{FlashcardMapper.ConvertToDto(flashcard).Back}");
                                                }

                                                AnsiConsole.Write(table);

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
                                        case "Delete a Flashcard in the Stack":
                                            //FIX
                                            var inputDelete = UserInput.ChooseFlashcard(flashcardController.Read(chosenStack.CardStackId));
                                            AnsiConsole.Write(Validation.Validate(() => UserInput.ChooseFlashcard(flashcardController.Read(chosenStack.CardStackId))),
                                                new Style(Color.LightCyan1));
                                          flashcardController.Delete(inputDelete.Id, chosenStack.CardStackId);
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
                case "Exit":
                    endSession = true;
                    break;
            }
        }
    }
}