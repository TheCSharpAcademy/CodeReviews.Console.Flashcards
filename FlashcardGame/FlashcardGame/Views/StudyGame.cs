using Dapper;
using FlashcardGame.Helpers;
using FlashcardGame.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace FlashcardGame.Views
{
    internal class StudyGame
    {
        static string Connection = Helper.CnnVal("FlashcardDatabase");

        public static void PickStackMenu()
        {
            Console.Clear();
            bool runFlashcardskMenu = true;
            while (runFlashcardskMenu)
            {
                Console.Clear();
                List<Stack> stacks = DataAccess.GetStacks();
                if (stacks.Count == 0)
                {
                    Console.WriteLine("No stacks were found");
                    Console.WriteLine("Press any key to return");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Choose what stack you want to study or press 0 to exit");

                Console.WriteLine("Options: ");



                for (int i = 0; i < stacks.Count; i++)
                {
                    Stack stack = stacks[i];
                    Console.WriteLine($"{i + 1}. {stack.stack_name}");
                }
                string option = "";
                bool notValidOption = true;
                while (notValidOption)
                {

                    Console.WriteLine("Please enter stack name");
                    option = Console.ReadLine();

                    if (option == "0")
                    {
                        return;
                    }


                    if (stacks.Any(s => s.stack_name == option))
                    {
                        notValidOption = false;
                        Console.WriteLine("Valid option selected.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Try again.");
                    }
                }

                int stackId = stacks.Single(s => s.stack_name == option).stack_id;

                RunStudyGame(stackId);


            }
        }
        public static void RunStudyGame(int stackId)
        {
            Console.Clear();
            var stacks = DataAccess.GetStacks();
            var flashcards = DataAccess.GetFlashcards(stackId);
            var chosenStack = new Stack();
            foreach (var stack in stacks)
            {
                if (stack.stack_id == stackId)
                {
                    chosenStack = stack;
                }
            }

            var filteredFlashcards = flashcards.Where(flashcard => flashcard.stack_Id == chosenStack.stack_id).ToList();
            int rightAnswers = 0;

            if (filteredFlashcards.Count == 0)
            {
                Console.WriteLine("Stack is empty!");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                return;
            }

            int badAnswers = 0;
            foreach (var flashcard in filteredFlashcards)
            {
                Console.Clear();
                Console.WriteLine($"Current stack of flashcards: {chosenStack.stack_name}");

                Console.WriteLine("Question:");

                Console.WriteLine(new string('-', flashcard.flashCard_Question.Length + 6));
                Console.WriteLine($"|--{flashcard.flashCard_Question}--|");
                Console.WriteLine(new string('-', flashcard.flashCard_Question.Length + 6));

                Console.WriteLine("Enter the answer");
                string? answer = Console.ReadLine();

                if (answer.ToLower() == flashcard.flashcard_Answer.ToLower())
                {
                    Console.WriteLine("You are correct!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    rightAnswers++;
                }
                else
                {
                    Console.WriteLine("You are wrong!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    badAnswers++;
                }
            }

            Console.WriteLine($"Well done you got {rightAnswers} right and {badAnswers} wrong answers!");
            Console.WriteLine("Press any key to return");
            Console.ReadLine();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string insertQuery =
                    $@"INSERT INTO StudySessionsTable (right_answers, bad_answers, stack_id, study_date) VALUES ('{rightAnswers}', '{badAnswers}', '{chosenStack.stack_id}', '{DateTime.UtcNow}')";

                connection.Execute(insertQuery);
            }
        }
    }
}
