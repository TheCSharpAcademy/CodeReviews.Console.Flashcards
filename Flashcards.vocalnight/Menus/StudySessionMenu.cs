using ConsoleTableExt;
using Flashcards.CRUD;
using Flashcards.Helpers;
using Flashcards.Model;

namespace Flashcards.Menus {
    internal class StudySessionMenu {

        internal static void StudySession( Stack stack ) {

            int score = 0;

            List<CardDto> cards = DbOperations.GetFlashcards(stack);

            foreach (CardDto card in cards) {

                var table = new List<List<object>> {
                    new List<object> { card.Front }
                };

                ConsoleTableBuilder
               .From(table)
               .WithColumn("Front")
               .ExportAndWriteLine();

                Console.WriteLine("Type your answer!\n");
                Console.WriteLine($"Current score: {score}");
                HelpersAndValidation.InsertSeparator();

                string op = Console.ReadLine();
                Console.Clear();

                if (op == card.Back) {
                    score++;
                    Console.WriteLine($"You got it right, the answer was {card.Back}");
                } else {
                    Console.WriteLine($"You got it wrong, the answer was {card.Back}");
                }

                Console.WriteLine("Press Enter or anything and then Enter to go next");
                Console.ReadLine();
            }

            DbOperations.SaveStudySessions(stack.Id, score);

            ManageStudySessionMenu.ManageStudySessions(stack);

        }
    }
}
