using ConsoleTableExt;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards
{
    internal static class MenuBuilders
    {
        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("\t++++++++++++++++++++++++++++++");
            Console.WriteLine();
            Console.WriteLine("\tQuit - press Q");
            Console.WriteLine("\tManage stacks - press 1");
            Console.WriteLine("\tManage flashcards - press 2");
            Console.WriteLine("\tStudy - press 3");
            Console.WriteLine("\tView study history - press 4");
            Console.WriteLine();
            Console.WriteLine("\t++++++++++++++++++++++++++++++\n");

            bool appIsRunning = true;
            string userInput = Console.ReadLine();
            while (appIsRunning)
            {
                if (userInput.ToLower() == "q")
                {
                    Console.WriteLine("\nGoodbye.");
                    appIsRunning = false;
                    Environment.Exit(0);
                    Console.ReadLine();
                }
                else if (userInput == "1")
                {
                    ManageStacksMenu();
                }
                else if (userInput == "2")
                {
                    ManageFlashcardsMenu();
                }
                else if (userInput == "3")
                {
                    StudyStacksMenu();
                }
                else if (userInput == "4")
                {
                    ViewStudyHistoryMenu();
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                    Console.ReadLine();
                }
            }
        }

        private static void ViewStudyHistoryMenu()
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;
            List<ScoreHistoryModel> results = new List<ScoreHistoryModel>();

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            sql = $"SELECT Study.Score, Study.Date, Stacks.Name " +
                $"FROM Study " +
                $"JOIN Stacks ON Study.StackId = Stacks.Id";

            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new ScoreHistoryModel
                {
                    Score = reader.GetString(0),
                    Date = reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                    Stackname = reader.GetString(2)
                });
            }

            cnn.Close();

            var listOfResults = new List<List<object>>();
            listOfResults.Add(new List<object> { "Score", "Date", "Subject" });

            foreach (ScoreHistoryModel scoreHistoryModel in results)
            {
                listOfResults.Add(new List<object> { scoreHistoryModel.Score, scoreHistoryModel.Date, scoreHistoryModel.Stackname });
            }
            ConsoleTableBuilder
                .From(listOfResults)
                .ExportAndWriteLine();

            Console.WriteLine("\nPress any key to return to main menu.");
            Console.ReadLine();
            MainMenu();
        }

        private static void StudyStacksMenu()
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;
            List<StackModel> stacks = new List<StackModel>();

            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = $"SELECT * FROM Stacks ORDER BY Id";

            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                stacks.Add(
                    new StackModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
            }
            var listOfStacks = new List<List<object>>();
            listOfStacks.Add(new List<object> { "Subject" });
            foreach (StackModel stack in stacks)
            {
                string displayString = $"{stack.Name}";
                listOfStacks.Add(new List<object> { displayString });
            }
            //print table
            ConsoleTableBuilder
                .From(listOfStacks)
                .ExportAndWriteLine();

            Console.WriteLine("\nChoose the subject you want to study. \"study <subject>\" is the command to use.");
            string userInput = Console.ReadLine();
            string[] words = userInput.Split(' ');
            string stackToStudy = words[1];
            if (words.Length != 2)
            {
                Console.WriteLine("Invalid input.");
                Console.ReadLine();
            }
            else
            {
                List<string> questions = new List<string>();
                List<string> answers = new List<string>();
                int totalScore = 0;
                int correctAnswers = 0;
                questions = Helpers.GetQuestions(stackToStudy);
                answers = Helpers.GetAnswers(stackToStudy);
                for (int i = 0; i < questions.Count; i++)
                {
                    totalScore++;
                    Console.WriteLine($"Question: {questions[i]}");
                    string userAnswer = Console.ReadLine();
                    if (userAnswer != answers[i])
                    {
                        Console.WriteLine("Wrong answer.");
                        Console.ReadLine();
                    }
                    else if (userAnswer == answers[i])
                    {
                        correctAnswers++;
                        Console.WriteLine("Correct!");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("?");
                    }
                }
                string totalScoreString = correctAnswers + "/" + totalScore;
                Helpers.WriteScoreToDb(totalScoreString, stackToStudy);

                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"Your score was {totalScoreString}.");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
                Console.ReadLine();
            }
        }

        private static void ManageFlashcardsMenu()
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;
            List<FlashcardDTO> flashcards = new List<FlashcardDTO>();

            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = $"SELECT f.Id, f.Question, f.Answer, s.Name " +
                $"FROM Flashcards f " +
                $"JOIN Stacks s ON f.StackId = s.Id;";

            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                flashcards.Add(
                    new FlashcardDTO
                    {
                        Id = reader.GetInt32(0),
                        Question = reader.GetString(1),
                        Answer = reader.GetString(2),
                        StackName = reader.GetString(3)
                    });
            }
            var listOfFlashcards = new List<List<object>>();
            foreach (FlashcardDTO flashcard in flashcards)
            {
                listOfFlashcards.Add(new List<object> { flashcard.Id, flashcard.Question, flashcard.Answer, flashcard.StackName });
            }
            //print table
            ConsoleTableBuilder
                .From(listOfFlashcards)
                .ExportAndWriteLine();

            //user input
            bool isValidInput = false;
            while (isValidInput == false)
            {
                Console.WriteLine("\n+++++++++++++++++++++++++++++++++");
                Console.WriteLine("Type \"add <question> <answer> <stackname>\" to add a new flashcard");
                Console.WriteLine("Type \"delete <flashcardid>\" to delete a flashcard");
                Console.WriteLine("Type \"update <flashcardid> <newquestion> <newanswer> <newstackname>\" to update a flashcard");
                Console.WriteLine("Or type 0 to exit to main menu");
                Console.WriteLine("+++++++++++++++++++++++++++++++++\n");
                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "0")
                {
                    isValidInput = true;
                    MainMenu();
                }
                else if (userInput.ToLower().Substring(0, 3) == "add")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 4)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string newQuestion = char.ToUpper(words[1][0]) + words[1].Substring(1);
                        string newAnswer = char.ToUpper(words[2][0]) + words[2].Substring(1);
                        string newStackName = char.ToUpper(words[3][0]) + words[3].Substring(1);

                        Flashcards.CreateNewFlashcard(newQuestion, newAnswer, newStackName);
                        isValidInput = true;
                    }
                }
                else if (userInput.ToLower().Substring(0, 6) == "delete")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 2)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string deleteFlashcardId = char.ToUpper(userInput[7]) + userInput.Substring(8);
                        int parsedFlashcardId = 0;
                        if (int.TryParse(deleteFlashcardId, out parsedFlashcardId))
                        {
                            Flashcards.DeleteFlashCard(parsedFlashcardId);
                        }
                        else
                        {
                            Console.WriteLine("That's not a valid ID to delete.");
                            Console.ReadLine();
                        }
                        isValidInput = true;
                    }
                }
                else if (userInput.ToLower().Substring(0, 6) == "update")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 5)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string updateFlashcardId = words[1];
                        int parsedFlashcardId = 0;
                        if (int.TryParse(updateFlashcardId, out parsedFlashcardId))
                        {
                            string updateQuestion = char.ToUpper(words[2][0]) + words[2].Substring(1);
                            string updateAnswer = char.ToUpper(words[3][0]) + words[3].Substring(1);
                            string updateStackname = char.ToUpper(words[4][0]) + words[4].Substring(1);
                            Flashcards.UpdateFlashCard(parsedFlashcardId, updateQuestion, updateAnswer, updateStackname);
                        }
                        else
                        {
                            Console.WriteLine("That's not a valid ID to update.");
                            Console.ReadLine();
                        }
                        isValidInput = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, try again.");
                }
                isValidInput = true;
            }
            cnn.Close();
        }

        internal static void ManageStacksMenu()
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;
            List<StackModel> stacks = new List<StackModel>();

            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = $"SELECT * FROM Stacks ORDER BY Id";

            command = new SqlCommand(sql, cnn);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                stacks.Add(
                    new StackModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
            }
            var listOfStacks = new List<List<object>>();
            listOfStacks.Add(new List<object> { "Stackname" });
            foreach (StackModel stack in stacks)
            {
                string displayString = $"{stack.Name}";
                listOfStacks.Add(new List<object> { displayString });
            }
            //print table
            ConsoleTableBuilder
                .From(listOfStacks)
                .ExportAndWriteLine();

            //user input
            bool isValidInput = false;
            while (isValidInput == false)
            {
                Console.WriteLine("\n+++++++++++++++++++++++++++++++++");
                Console.WriteLine("Type \"open <stackname>\" to open it");
                Console.WriteLine("Type \"add <stackname>\" to add a new stack");
                Console.WriteLine("Type \"delete <stackname>\" to delete a stack");
                Console.WriteLine("Type \"update <stackname> <newstackname>\" to update a stack");
                Console.WriteLine("Or type 0 to exit to main menu");
                Console.WriteLine("+++++++++++++++++++++++++++++++++\n");
                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "0")
                {
                    isValidInput = true;
                    MainMenu();
                }
                else if (userInput.ToLower().Substring(0, 3) == "add")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 2)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string newStackname = char.ToUpper(userInput[4]) + userInput.Substring(5);
                        Stacks.CreateNewStack(newStackname);
                        isValidInput = true;
                    }
                }
                else if (userInput.ToLower().Substring(0, 4) == "open")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 2)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string openStackname = char.ToUpper(userInput[5]) + userInput.Substring(6);
                        Stacks.StackSubMenu(openStackname);
                        isValidInput = true;
                    }
                }
                else if (userInput.ToLower().Substring(0, 6) == "delete")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 2)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string deleteStackname = char.ToUpper(userInput[7]) + userInput.Substring(8);
                        Stacks.DeleteStack(deleteStackname);
                        isValidInput = true;
                    }
                }
                else if (userInput.ToLower().Substring(0, 6) == "update")
                {
                    string[] words = userInput.Split(' ');
                    if (words.Length != 3)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.ReadLine();
                    }
                    else
                    {
                        string updateStackname = char.ToUpper(words[1][0]) + words[1].Substring(1);
                        string newStackname = char.ToUpper(words[2][0]) + words[2].Substring(1);
                        Stacks.UpdateStack(updateStackname, newStackname);
                        isValidInput = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input, try again.");
                }
                isValidInput = true;
            }
            cnn.Close();
        }
    }
}
