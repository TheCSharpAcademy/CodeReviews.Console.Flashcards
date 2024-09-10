using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Flashcards;


namespace Flashcards
{


    public class StudyApp
    {
        string connectionString="Server=localhost;Database=FLASHCARDS;User ID=SA;Password=Password123;";

        public void StudyMenu(){
            Console.WriteLine("\t\t-----STUDY SESSION-----");
            Console.WriteLine(" TYPE 1 TO START A STUDY SESSION");
            Console.WriteLine(" TYPE 2 TO SEE STUDY SESSIONS");

            string choice = Console.ReadLine();
            switch(choice){
                case "1":
                    Start();
                    break;
                case "2":
                    ShowStudySessions();
                    break;
            }
        }

        public void ShowStudySessions()
        {
            using(SqlConnection connection = new SqlConnection(connectionString)){
                connection.Open();
                var table = connection.Query("SELECT * FROM dbo.StudySession");

                foreach(var row in table){
                    Console.WriteLine($"Id: {row.Id} \t Date: {row.Date} \t Score: {row.Score}");
                }
                Console.ReadLine();

            }
        }

        public List<FlashcardsDTO> QuestionsAndAnswers(int stackID){
            using(SqlConnection connection = new SqlConnection(connectionString)){
                    connection.Open();

                    string query = "SELECT Question, Answer FROM dbo.FLASHCARD WHERE StackID = @StackID";

                    var flashcards = connection.Query<FlashcardsDTO>(query, new{StackID = stackID}).ToList();

                    return flashcards;
                }
        }

        public void Start(){

            DateTime date = DateTime.Now;
            var score = 0;

            Console.WriteLine("CHOOSE STACK ID");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (QuestionsAndAnswers(choice).Any())
            {
                foreach (var flashcard in QuestionsAndAnswers(choice))
                {
                    Console.WriteLine($"The question is {flashcard.Question}");
                    var answer = Console.ReadLine();

                    if (answer == flashcard.Answer)
                    {
                        Console.WriteLine("CORRECT");
                        score++;
                    }
                    else
                    {
                        Console.WriteLine("INCORRECT");
                    }
                }

                Console.WriteLine($"CONGRATULATIONS, YOUR SCORE IS {score}");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                
                string query = "INSERT INTO dbo.StudySession (Date, Score, StackID) VALUES (@Date, @Score, @StackID)";
                var parameters = new { Date = date, Score = score, StackID = choice };  
                connection.Execute(query, parameters);
            }
        }


       
    }
}