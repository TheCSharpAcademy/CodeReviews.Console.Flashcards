using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    var chosenStack = UserInput.ChooseStack(cardStackController.Read());
                    var t = studyController.StartSession(chosenStack, flashcardController);
                    if(t != null) studyController.Create(t);
                    Validation.EndMessage("");
                    break;
            }
        }
    }
}
