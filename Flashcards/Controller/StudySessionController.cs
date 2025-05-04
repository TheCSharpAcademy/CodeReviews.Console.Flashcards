using Flashcards.Model;
using Flashcards.View;
using Spectre.Console;

namespace Flashcards.Controller
{
    public class StudySessionController
    {
        public static void Study()
        {
            var (stack, _) = Display.PrintStackSelectionMenu("Study", "Select the stack of flashcards you want to study...");

            int stackId = stack.Id;

            AnsiConsole.Write(new Markup($"\n[dodgerblue1]Stack:[/] [white]{stack.Name}[/]\n"));

            int numberOfFlashcardsToStudy = UI.PromptForNumberOfFlashcards("\nHow many flashcards do you want to study during this session? ", stackId);

            Console.Clear();

            var flashcardsRepo = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
            var flashcards = flashcardsRepo.GetAllFlashcardsForStack(stackId);

            Random random = new Random();
            int correctAnswers = 0;

            List<int> shownIndices = new List<int>();

            for (int i = 0; i < numberOfFlashcardsToStudy; i++)
            {
                Console.Clear();
                Console.WriteLine("Studying...");

                int randomIndex;

                do
                {
                    randomIndex = random.Next(flashcards.Count);
                } while (shownIndices.Contains(randomIndex));

                FlashcardDto flashcard = flashcards[randomIndex];

                Console.WriteLine($"\n\t{flashcard.Question}\n");

                Console.Write("Your Answer: ");

                string? userAnswer = Console.ReadLine();

                if (userAnswer.Equals(flashcard.Answer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\n\tCorrect!");
                    correctAnswers++;
                }
                else
                {
                    Console.WriteLine($"\n\tIncorrect. The correct answer is: {flashcard.Answer}");
                }

                shownIndices.Add(randomIndex);

                if (i < numberOfFlashcardsToStudy - 1)
                {
                    Console.WriteLine("\nPress any key to continue to the next flashcard...");
                    Console.ReadKey();
                }
            }

            decimal percentageCorrect = (decimal)correctAnswers / numberOfFlashcardsToStudy * 100;

            var session = new StudySession
            {
                StackId = stackId,
                SessionStartTime = DateTime.Now,
                PercentageCorrect = percentageCorrect
            };

            var statsRepo = new StudySessionRepository(DatabaseUtility.GetConnectionString());
            statsRepo.SaveStudySessionStats(session);
        }
    }
}
