using Flashcards.TwilightSaw.Controller;
using Flashcards.TwilightSaw.Helpers;

namespace Flashcards.TwilightSaw.View;

public class StackMenu(FlashcardController flashcardController, CardStackController cardStackController)
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
                    CreateStack();
                    break;
                case "Choose a Stack":
                    ChooseStack();
                    break;
                case "Delete a Stack":
                    DeleteStack();
                    break;
                case "Return":
                    endManage = true;
                    break;
            }
        }
    }

    private void DeleteStack()
    {
        var inputStackDelete = UserInput.ChooseStack(cardStackController.Read());
        var executeStackDelete = Validation.Validate(() => cardStackController.Delete(inputStackDelete));
        Validation.EndMessage(inputStackDelete == null || inputStackDelete.Name == "Return" ? null : executeStackDelete);
    }

    private void ChooseStack()
    {
        var chosenStack = UserInput.ChooseStack(cardStackController.Read());
        if (chosenStack == null || chosenStack.Name == "Return") return;
        new FlashcardMenu(flashcardController, chosenStack).Menu();
    }

    private void CreateStack()
    {
        var stackName = UserInput.Create("Name your new Stack");
        if (stackName == "0") return;
        cardStackController.Create(stackName);
        Validation.EndMessage("");
    }
}