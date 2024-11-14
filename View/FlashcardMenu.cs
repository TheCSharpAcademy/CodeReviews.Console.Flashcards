using Flashcards.TwilightSaw.Controller;
using Flashcards.TwilightSaw.Helpers;
using Flashcards.TwilightSaw.Models;
using Spectre.Console;

namespace Flashcards.TwilightSaw.View;

public class FlashcardMenu(FlashcardController flashcardController, CardStack chosenStack)
{
    public void Menu()
    {
        var endStack = false;
        while (!endStack)
        {
            var inputCreateStacks = UserInput.CreateChoosingList([
                "Create a Flashcard in the Stack", "View all Flashcards in the Stack", "Edit a Flashcard in the Stack",
                "Delete a Flashcard in the Stack", "Return"
            ]);
            switch (inputCreateStacks)
            {
                case "Create a Flashcard in the Stack":
                    CreateFlashcard();
                    break;
                case "View all Flashcards in the Stack":
                    ViewStackFlashcards();
                    break;
                case "Edit a Flashcard in the Stack":
                    EditFlashcard();
                    break;
                case "Delete a Flashcard in the Stack":
                    DeleteFlashcard();
                    break;
                case "Return":
                    endStack = true;
                    break;
            }
        }
    }

    private void DeleteFlashcard()
    {
        var inputDelete = flashcardController.GetFlashcard(flashcardController, chosenStack);
        if (inputDelete == null)
        {
            Validation.EndMessage("No Flashcards in the current Stack!");
            return;
        }
        if (inputDelete.Front == "Return") return;
        var executeDelete =
            Validation.Validate(() => flashcardController.Delete(inputDelete.Id, chosenStack.CardStackId));
        Validation.EndMessage(executeDelete);
    }

    private void EditFlashcard()
    {
        var inputUpdate = flashcardController.GetFlashcard(flashcardController, chosenStack);
        if (inputUpdate == null)
        {
            Validation.EndMessage("No Flashcards in the current Stack!");
            return;
        }
        if (inputUpdate.Front == "Return") return;

        var inputUpdateSide = UserInput.CreateChoosingList(["Front side", "Back side"]);
        var newFlashcardSide =
            UserInput.Create(inputUpdateSide == "Front side" ? "Type your new Front Side" : "Type your new Back Side");
        if (newFlashcardSide == "0") return;

        var executeUpdate = Validation.Validate(() =>
            flashcardController.Update(inputUpdate.Id, chosenStack.CardStackId, (inputUpdateSide, newFlashcardSide)));
        Validation.EndMessage(executeUpdate);
    }

    private void ViewStackFlashcards()
    {
        var read = flashcardController.Read(chosenStack.CardStackId);
        AnsiConsole.Write(UserInput.CreateFlashcardTable(read));
        Validation.EndMessage("");
    }

    private void CreateFlashcard()
    {
        var frontSideInput = UserInput.Create("Name the front side of your Flashcard");
        if (frontSideInput == "0") return;
        var backSideInput = UserInput.Create("Name the back side of your Flashcard");
        if (backSideInput == "0") return;
        flashcardController.Create(frontSideInput, backSideInput, chosenStack.CardStackId);
        Validation.EndMessage("Created successfully");
    }
}