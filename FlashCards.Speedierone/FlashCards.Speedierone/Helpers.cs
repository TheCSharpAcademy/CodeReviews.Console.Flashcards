using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Globalization;
namespace FlashCards;
internal class Helpers
{
    internal static void AddToHistory(int gameScore, string gameSubject, int gameAmount)
    {
        try
        {
            DateTime date = DateTime.Now;
            string dateString = date.ToString("dd-MM-yyyy");
            DateTime dateFormat = DateTime.ParseExact(dateString, "dd-MM-yyyy", new CultureInfo("en-GB"));
            
            var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
            "INSERT INTO StudyGames (Date, Subject, GameScore, GameAmount) VALUES (@date, @subject, @score, @amount)";
                tableCmd.Parameters.AddWithValue("@date", dateFormat);
                tableCmd.Parameters.AddWithValue("@subject", gameSubject);
                tableCmd.Parameters.AddWithValue("@score", gameScore);
                tableCmd.Parameters.AddWithValue("@amount", gameAmount);
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.ToString() + "Press any key to continue.");
            Console.ReadLine();
        }
    }
    internal static void CheckSubjectExists(string @subject)
    {
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.Parameters.AddWithValue("@subject", subject);
            tableCmd.CommandText =
                $"SELECT COUNT (*) FROM Stack WHERE Subject = @subject";
            int count = (int)tableCmd.ExecuteScalar();
            if (count == 0)
            {
                Console.WriteLine("No subject of that name exists. Press any button to continue.");
                Console.ReadLine();
                MainMenu.ShowMenu();
            }
        }
    }
    internal static void CheckSubjectDuplicate(string @subject)
    {
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.Parameters.AddWithValue("@subject", subject);
            tableCmd.CommandText =
                $"SELECT COUNT (*) FROM Stack WHERE Subject = @subject";
            int count = (int)tableCmd.ExecuteScalar();
            if (count == 1)
            {
                Console.WriteLine("Subject already exists. Press any button to return to main menu.");
                Console.ReadLine();
                MainMenu.ShowMenu();
            }
        }
    }
    internal static int CreateCustomId()
    {
        int nextID = 1;
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT COALESCE(MAX(ID), 0) FROM Questions";
            var result = tableCmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                nextID = Convert.ToInt32(result) + 1;
                
            }
            return nextID;
        }       
    }
}

