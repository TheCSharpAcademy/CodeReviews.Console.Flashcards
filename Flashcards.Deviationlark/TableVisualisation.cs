using Spectre.Console;
namespace Flashcards
{
    class TableVisualisation
    {
        internal void ShowStacks(List<StackModel> stacks)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");
            int stackId = 1;

            foreach (var stack in stacks)
            {
                table.AddRow($"{stackId}", $"{stack.Name}");
                stackId++;
            }

            AnsiConsole.Write(table);
        }
        internal void ShowFlashcards(List<FlashcardModel> flashcards)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Front");
            table.AddColumn("Back");
            int flashcardId = 1;

            foreach (var flashcard in flashcards)
            {
                table.AddRow($"{flashcardId}", $"{flashcard.Front}", $"{flashcard.Back}");
                flashcardId++;
            }
            AnsiConsole.Write(table);
        }

        internal void ShowStudyHistory(List<StudyModel> studyHistory)
        {
            var table = new Table();

            table.AddColumn("Id");
            table.AddColumn("Date");
            table.AddColumn("Score");
            int studyId = 1;

            foreach(var record in studyHistory)
            {
                table.AddRow($"{studyId}", $"{record.Date}", $"{record.Score}");
                studyId++;
            }
            AnsiConsole.Write(table);
            Console.WriteLine("Press enter to go back to main menu.");
            Console.ReadLine();
        }

        internal void ShowRandomFlashcards(List<FlashcardModel> flashcards)
        {
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            Random random = new();
            StudyController studyController = new();
            int numQuestions = flashcards.Count;
            int score = 0;
            var table = new Table();
            table.AddColumn("Front");

            while (flashcards.Count > 0)
            {
                Console.Clear();
                int randomNum = random.Next(0, flashcards.Count);
                table.AddRow($"{flashcards[randomNum].Front}");
                AnsiConsole.Write(table);
                Console.WriteLine("Enter the answer to this card: ");
                string? userInput = Console.ReadLine();

                if (userInput.ToLower() != flashcards[randomNum].Back.ToLower())
                {
                    Console.WriteLine("Your answer is wrong.");
                    Console.WriteLine($"Your answer: {userInput}");
                    Console.WriteLine($"The correct answer: {flashcards[randomNum].Back}");
                    Console.WriteLine("Press enter to go to the next flashcard.");
                    Console.ReadLine();
                }
                else 
                {
                    Console.WriteLine("Correct! +1 point");
                    Console.WriteLine("Press enter to go to the next flashcard.");
                    Console.ReadLine();
                    score++;
                }
                flashcards.Remove(flashcards[randomNum]);
                Console.WriteLine("Score: " + score);
                table.RemoveRow(0);
            }
            Console.WriteLine($"Study over. Score: {score}/{numQuestions}");
            studyController.Insert(score, date);
            Console.WriteLine("Press enter to go back to main menu.");
            Console.ReadLine();
        }
    }
}