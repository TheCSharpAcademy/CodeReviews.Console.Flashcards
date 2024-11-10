using Flashcards.TwilightSaw.Controller;
using Spectre.Console;

namespace Flashcards.TwilightSaw.View;

public class StackMenu(AppDbContext context, FlashcardController flashcardController, CardStackController cardStackController)
{
    public void Menu()
    {
        var endManage = false;
        while (!endManage)
        {
            var inputStacks = UserInput.CreateChoosingList(["Choose a Stack", "Create a new Stack", "Delete a Stack", "Return"]);
            switch (inputStacks)
            {
                case "Create a new Stack":
                    var stackName = UserInput.Create("Name your new Stack");
                    if (stackName == "0") break;
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
                                var frontSideInput = UserInput.Create("Name the front side of your Flashcard");
                                if (frontSideInput == "0") break;
                                var backSideInput = UserInput.Create("Name the back side of your Flashcard");
                                if (backSideInput == "0") break;
                                flashcardController.Create(frontSideInput, backSideInput, chosenStack.CardStackId);
                                Console.Clear();
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
                                var inputUpdate = flashcardController.GetFlashcard(flashcardController, chosenStack);
                                if(inputUpdate != null)
                                {
                                    var inputUpdateSide = UserInput.CreateChoosingList(["Front side", "Back side"]);
                                    var newFlashcardSide = UserInput.Create(inputUpdateSide == "Front side" ? "Type your new Front Side" : "Type your new Back Side");
                                    if (newFlashcardSide == "0") break;
                                    var executeUpdate = Validation.Validate(() => flashcardController.Update(inputUpdate.Id, chosenStack.CardStackId, (inputUpdateSide, newFlashcardSide)));
                                    Validation.EndMessage(executeUpdate);
                                }
                                else
                                    Validation.EndMessage("No Flashcards in the current Stack!");
                                break;
                            case "Delete a Flashcard in the Stack":
                                var inputDelete = flashcardController.GetFlashcard(flashcardController, chosenStack);
                                var executeDelete = Validation.Validate(() => flashcardController.Delete(inputDelete.Id, chosenStack.CardStackId));
                                Validation.EndMessage(executeDelete);
                                break;
                            case "Return":
                                endStack = true;
                                break;
                        }
                    }
                    break;
                case "Delete a Stack":
                    var inputStackDelete = UserInput.ChooseStack(cardStackController.Read());
                    var executeStackDelete = Validation.Validate(() => cardStackController.Delete(inputStackDelete));
                    Validation.EndMessage(executeStackDelete);
                    break;
                case "Return":
                    endManage = true;
                    break;
            }
        }
    }
}