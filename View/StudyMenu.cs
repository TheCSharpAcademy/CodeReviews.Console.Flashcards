using Flashcards.TwilightSaw.Controller;

namespace Flashcards.TwilightSaw.View
{
    internal class StudyMenu(AppDbContext context, FlashcardController flashcardController)
    {
        private StudyController studyController = new(context);
        public void Menu(CardStackController cardStackController)
        {
            var inputStudy = UserInput.CreateChoosingList(["Pick a Stack to study"]);
            switch (inputStudy)
            {
                case "Pick a Stack to study":
                    var stackList = cardStackController.Read();
                    if (stackList != null)
                    {
                        var chosenStack = UserInput.ChooseStack(stackList);
                        var t = studyController.StartSession(chosenStack, flashcardController);
                        if (t != null) studyController.Create(t);
                    }
                    Validation.EndMessage(stackList == null ? "No Stack has been created yet" : "");
                    break;
            }
        }
    }
}
