using System.Data.SqlClient;
using Dapper;

namespace FlashcardGame
{
    internal class DatabaseHelpers
    {
        static string Connection = Helper.CnnVal("Dbtest");

        public static void DeleteStack()
        {
            Console.Clear();
            Console.WriteLine("Write a name of stack that you want to Delete or press 0 to exit:");

            List<Stack> stacks = DataAccess.GetStacks();
            
            

            if (stacks.Count == 0)
            {
                Console.WriteLine("No stacks were found!");
                return;
            }

            for (int i = 0; i < stacks.Count; i++)
            {
                Stack stack = stacks[i];
                Console.WriteLine($"{i + 1}. {stack.stack_name}");
            }

            string? stackName = Console.ReadLine();

            if (stackName == "0")
            {
                return;
            }

           
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string deleteQuery =
                    $@"DELETE FROM StacksTable WHERE CAST(stack_name AS VARCHAR(MAX)) = ('{stackName}')";

                int count = connection.Execute(deleteQuery);

                if (count > 0)
                {
                    Console.WriteLine("Row was deleted!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No row was deleted, because name doesn't match that you entered!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
            }
        }
        public static void AddStack()
        {
            Console.Clear();
            Console.WriteLine("Write a name of stack that you want to add or press 0 to exit:");

            string stackName = Console.ReadLine();

            if (stackName == "0")
            {
                return;
            }
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string insertQuery = $@"INSERT INTO StacksTable (stack_name) VALUES ('{stackName}')";

                connection.Execute(insertQuery);
            }
        }

        public static void AddFlashcard(int stackId)
        {
            Console.Clear();
            Console.WriteLine("Write a question of flashcard you want to put in or press 0 to exit:");

            var question = Console.ReadLine();

            if (question == "0")
            {
                return;
            }
            while (string.IsNullOrEmpty(question))
            {
                Console.WriteLine("Question can't be empty");
                question = Console.ReadLine();

            }

            Console.WriteLine("Write an answer of the question or press 0 to exit");

            var answer = Console.ReadLine();
            
            if (answer == "0")
            {
                return;
            }

            while (string.IsNullOrEmpty(question))
            {
                Console.WriteLine("Answer can't be empty");
                answer = Console.ReadLine();

            }

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string insertQuery =
                    $@"INSERT INTO FlashcardTable (flashCard_Question, flashcard_Answer, stack_Id ) VALUES ('{question}', '{answer}', '{stackId}');";

                connection.Execute(insertQuery);
            }
        }

        public static void DeleteFlashcard(int stackId)
        {
            ViewFlashcard(stackId);
            Console.WriteLine("Write a id of flashcard that you want to delete or press 0 to exit:");
            string? flashcardId = Console.ReadLine();
            int id;

            if (flashcardId == "0")
            {
                return;
            }

            while (!int.TryParse(flashcardId, out id))
            {
                Console.WriteLine("Please enter integer. Try again ");
                flashcardId = Console.ReadLine();
            }

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string deleteQuery =
                    $@"DELETE FROM FlashcardTable WHERE flashcard_Id = ('{id}')";

                int count = connection.Execute(deleteQuery);

                if (count > 0)
                {
                    Console.WriteLine("Row was deleted!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No row was deleted, because id doesn't match that you entered!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
            }
        }

        public static void UpdateFlashcard(int stackId)
        {
            

            ViewFlashcard(stackId);

            Console.WriteLine("Write a id of flashcard that you want to edit or press 0 to exit:");
            string? flashcardId = Console.ReadLine();
            int id;

            if (flashcardId == "0")
            {
                return;
            }

            while (!int.TryParse(flashcardId, out id))
            {
                Console.WriteLine("Please enter integer. Try again ");
                flashcardId = Console.ReadLine();
            }
            Console.WriteLine("Write a new question or press 0 to exit");
            var newQuestion = Console.ReadLine();

            if (newQuestion == "0")
            {
                return;
            }
            while (string.IsNullOrEmpty(newQuestion))
            {
                Console.WriteLine("Question can't be empty");
                newQuestion = Console.ReadLine();

            }

            Console.WriteLine("Write an answer of the question or press 0 to exit");

            var newAnswer = Console.ReadLine();

            if (newQuestion == "0")
            {
                return;
            }
            while (string.IsNullOrEmpty(newAnswer))
            {
                Console.WriteLine("Answer can't be empty");
                newAnswer = Console.ReadLine();

            }

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string insertQuery = $@"UPDATE FlashcardTable
                SET flashCard_Question = '{newQuestion}' , flashcard_Answer = '{newAnswer}'
                WHERE flashCard_Id = {id}";

                connection.Execute(insertQuery);
            }
        }

        public static void ViewFlashcard(int stackId)
        {
            Console.Clear();
            Console.WriteLine("========== Flashcards ==========\n");

            var flashcards = DataAccess.GetFlashcards();
            var filteredFlashcards = FilterFlashcardsByStackId(stackId, flashcards);
            
            if (filteredFlashcards.Count == 0)
            {
                Console.WriteLine("No flashcards found in this stack.");
                Console.WriteLine("Press any key to return to menu");
                Console.ReadLine();
                StackMenu.RunStackMenu();
            }


            Console.WriteLine("  Id    |    Question                            |    Answer");
            Console.WriteLine("--------|----------------------------------------|-------------------");

            for (int i = 0; i < filteredFlashcards.Count; i++)
            {
                FlashcardDTO flashcard = filteredFlashcards.ElementAt(i).ConvertToDTO();

                Console.WriteLine(
                    $" {flashcard.flashcard_Id,-6} | {flashcard.flashCard_Question,-38} | {flashcard.flashcard_Answer,-15}");

                Console.WriteLine("--------|----------------------------------------|-------------------");
            }

            Console.WriteLine("\n============================================");

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        public static List<Flashcard> FilterFlashcardsByStackId(int stackId, List<Flashcard> flashcards)
        {
            return flashcards.Where(flashcard => flashcard.stack_Id == stackId).ToList();
        }

        public static void ViewStudySessions()
        {
            var studySessions = DataAccess.GetStudySessions();

            if (studySessions == null || studySessions.Count == 0)
            {
                Console.WriteLine("No study sessions found.");
                return;
            }

            Console.WriteLine("========== Study Sessions ==========\n");

            Console.WriteLine(" Id   | Right Ans  | Wrong Ans  | Study Date            ");
            Console.WriteLine("------|------------|------------|-----------------------");

            foreach (var studySession in studySessions)
            {
                
                Console.WriteLine(studySession.ToString());
            }

            Console.WriteLine("\n====================================");

            Console.WriteLine("Press any key to continue:");
            Console.ReadLine();

        }


          
        
    }
}
