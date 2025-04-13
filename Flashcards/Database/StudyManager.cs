using Flashcards.Object_Classes;
using Flashcards.Validation;

namespace Flashcards.Database
{
    public static class StudyManager
    {
        public static void StartStudySession()
        {
            DateTime startTime;
            Random rng = new Random();
            if (!DatabaseManager.DoWeHaveFlashcardStacks())
            {
                Console.WriteLine("No flashcard stacks found. Please create a stack prior to studying.");
                return;
            }
            // Logic to start a study session
            // This could involve initializing a new StudySession object and saving it to the database
            StackManager.ViewAllFlashcardStacks();

            int stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you want to study, or type 0 to return to the Main Menu.");

            while (stackID != 0 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("StackTable"),
                               "StackID", stackID) == false)
            {
                Console.WriteLine("Please provide a valid Stack ID.");
                stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you want to study, or type 0 to return to the Main Menu.");
            }
            string stackName = DatabaseManager.GetStackName(stackID);
            List<FlashcardDTO> flashcards = DatabaseManager.GetFlashcards(stackID);
            rng = new Random();
            List<FlashcardDTO> shuffledCards = flashcards.OrderBy(a => rng.Next()).ToList();

            if (shuffledCards.Count == 0)
            {
                Console.WriteLine("No flashcards found in this stack. Please create flashcards first.");
                return;
            }

            Console.WriteLine($"This deck has {shuffledCards.Count} cards. Press any key to begin.");
            Console.ReadKey();
            Console.Clear();

            startTime = DateTime.Now;
            int score = PlayStudySession(shuffledCards);
            int duration = (int)DateTime.Now.Subtract(startTime).TotalMinutes;
            Console.WriteLine($"You studied for {duration} minutes.");
            DatabaseManager.CreateStudySession(score, shuffledCards.Count, stackID, stackName, startTime, duration);
        }

        public static int PlayStudySession(List<FlashcardDTO> shuffledCards)
        {
            string answer = "";
            int score = 0;
            foreach(FlashcardDTO card in shuffledCards)
            {
                Console.WriteLine($"Question: {card.Question}");
                Console.Write("Answer: ");
                answer = Console.ReadLine();
                while (InputValidation.IsValidString(answer) == false)
                {
                    Console.WriteLine("Please provide a valid answer.");
                    answer = Console.ReadLine();
                }

                if(answer == card.Answer)
                {
                    Console.WriteLine("Correct!");                    
                    score++;
                }
                else
                {
                    Console.WriteLine($"Incorrect. The correct answer is: {card.Answer}");
                }
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                Console.Clear();
            }
            Console.WriteLine($"You got {score} out of {shuffledCards.Count} correct.");
            if(score == shuffledCards.Count)
            {
                Console.WriteLine("Perfect score! Well done!");
            }
            else if (score > shuffledCards.Count / 2)
            {
                Console.WriteLine("Good job! You passed.");
            }
            else
            {
                Console.WriteLine("Better luck next time!");
            }
            return score;
        }

        public static void ViewStudySessions()
        {
            if (!DatabaseManager.DoWeHaveStudySessions())
            {
                Console.WriteLine("No sessions found. Please complete a session first.");
                return;
            }
            int studyID = InputValidation.GetQuantity("Enter the ID of the Study Session you wish to view, type -1 to view all Study Sessions \n or type 0 to return to the Main Menu.");
            while (studyID != 0 && studyID != -1 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("StudyTable"),
                "StudyID", studyID) == false)
            {
                Console.WriteLine("Please provide a valid Study Session ID.");
                studyID = InputValidation.GetQuantity("Enter the ID of the Study Session you wish to view, type -1 to view all Study Sessions \n or type 0 to return to the Main Menu.");
            }
            List<StudySessionDTO> studySessions = DatabaseManager.GetStudySessions(studyID);
            Console.WriteLine("----------------------------------------------\n");
            foreach (var session in studySessions)
            {
                Console.WriteLine($"Stack Name: {session.StackName} | ID: {session.StudySessionId} | Score: {session.Score} | Total Questions: {session.TotalQuestions} | Date: {session.SessionDate} | Duration (Min): {session.SessionDuration}");
            }
            Console.WriteLine("----------------------------------------------\n");
        }
    }
}