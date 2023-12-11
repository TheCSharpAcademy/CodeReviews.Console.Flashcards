using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FlashCards;
internal class StudyGame
{
    internal static void FlashGame()
    {
        UserInput.ViewStacks();
        Console.WriteLine("Please enter subject you would like to study.");
        var subject = Console.ReadLine();
        var score = 0;
        var questionLength = 0;
        if (subject == "0") MainMenu.ShowMenu();
        try
        {
            var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT FlashSubject, FrontOfCard, BackOfCard FROM Questions WHERE FlashSubject = '{subject}'";
                SqlDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var question = reader["FrontOfCard"].ToString();
                        var correctAnswer = reader["BackOfCard"].ToString();
                        Console.Clear();
                        Console.WriteLine($"{subject}.\n");
                        Console.WriteLine(question);                       
                        var userAnswer = Console.ReadLine();

                        if (string.Equals(userAnswer, correctAnswer, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Correct!");
                            Console.WriteLine("Press any key to continue.");
                            Console.ReadLine();
                            score++;
                            questionLength++;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Answer.\n");
                            Console.WriteLine($"Correct answer is: {correctAnswer}");
                            Console.WriteLine("\nPress any button to continue.");
                            Console.ReadLine();
                            questionLength++;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No subject of this name available. Press any button to try again.");
                    Console.ReadLine();
                    FlashGame();
                }          
                connection.Close();               
            }           
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.ToString() + "Please try again.");
            FlashGame();
        }
        Console.WriteLine($"You got {score} out of {questionLength} right. Press any key to continue.");
        Console.ReadLine();
        Helpers.AddToHistory(score, subject, questionLength);
    }
}

