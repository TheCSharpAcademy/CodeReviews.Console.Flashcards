using Flashcards.TwilightSaw.Controller;
using Flashcards.TwilightSaw.Helpers;

namespace Flashcards.TwilightSaw.View
{
    internal class StudyMenu(StudyController studyController, FlashcardController flashcardController, CardStackController cardStackController)
    {
        public void Menu()
        {
            Console.Clear();
            var inputStudy = UserInput.CreateChoosingList(["Pick a Stack to study"]);
            switch (inputStudy)
            {
                case "Pick a Stack to study":
                    StudyStack();
                    break;
            }
        }

        private void StudyStack()
        {
            var stackList = cardStackController.Read();
            if (stackList != null)
            {
                var chosenStack = UserInput.ChooseStack(stackList);
                var t = studyController.StartSession(chosenStack, flashcardController);
                if (t != null) studyController.Create(t);
            }
            Validation.EndMessage(stackList == null ? "No Stack has been created yet" : "");
        }
    }
}
