using LucianoNicolasArrieta.Flashcards.Persistence;

namespace LucianoNicolasArrieta.Flashcards.Model
{
    public class StudySession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }

        public Stack Stack { get; set; }

        public void RunSession(Stack stack)
        {
            Console.WriteLine($"You are working on {stack.Subject} stack");
            bool finished = false;
            FlashcardRepository flashcardRepository = new FlashcardRepository();
            List<Flashcard> flashcards = flashcardRepository.GetAllFromStack(stack.Id);
            var random = new Random();
            int random_index, correctAnswers = 0;
            int total = 0;

            while (!finished)
            {
                random_index = random.Next(flashcards.Count);
                Flashcard flashcard = flashcards[random_index];
                flashcards.Remove(flashcard);
                Console.WriteLine($"\nFlashcard question: {flashcard.Question}");
                Console.Write("You answer (0 to finish the session): ");
                string answer = Console.ReadLine();
                if (answer == flashcard.Answer)
                {
                    correctAnswers++;
                    Console.WriteLine("Correct!");
                    total++;
                } else if (answer != flashcard.Answer && answer != "0")
                {
                    Console.WriteLine($"Wrong answer: The correct answer was: '{flashcard.Answer}'");
                    total++;
                }
                if (flashcards.Count == 0 || answer == "0")
                {
                    finished = true;
                }
            }

            Console.WriteLine($"You finished the Study Session. Congratulations you scored {correctAnswers} out of {total}!");

            CorrectAnswers = correctAnswers;
            TotalQuestions = total;
            Stack = stack;

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();

            SaveSession();
        }

        private void SaveSession()
        {
            StudySessionRepository studySessionRepository = new StudySessionRepository();

            this.Date = DateTime.Now;

            studySessionRepository.Insert(this);
        }

    }
}