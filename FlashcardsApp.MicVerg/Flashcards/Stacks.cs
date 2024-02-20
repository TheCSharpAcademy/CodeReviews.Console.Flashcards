using Microsoft.Data.SqlClient;
using System.Configuration;

namespace Flashcards
{
    internal class Stacks
    {
        internal static void CreateNewStack(string stackname)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            sql = $"INSERT INTO Stacks (Name) " +
                $"VALUES ('{stackname}')";

            command = new SqlCommand(sql, cnn);
            int rowsEdited = command.ExecuteNonQuery();

            Console.WriteLine($"You added {rowsEdited} rows.");

            cnn.Close();

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
        internal static void DeleteStack(string stackname)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            sql = $"DELETE FROM Stacks " +
                $"WHERE CONVERT(VARCHAR, Name) = '{stackname}'";

            command = new SqlCommand(sql, cnn);
            int rowsEdited = command.ExecuteNonQuery();

            Console.WriteLine($"You deleted {rowsEdited} rows.");

            cnn.Close();

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
        internal static void UpdateStack(string stackname, string newStackname)
        {
            Console.Clear();

            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader reader;
            string sql;

            cnn = new SqlConnection(connectionString);
            cnn.Open();

            sql = $"UPDATE Stacks " +
                $"SET Name = '{newStackname}'" +
                $"WHERE CONVERT(VARCHAR, Name) = '{stackname}'";

            command = new SqlCommand(sql, cnn);
            int rowsEdited = command.ExecuteNonQuery();

            Console.WriteLine($"You edited {rowsEdited} rows.");

            cnn.Close();

            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
        internal static void StackSubMenu(string stackname)
        {
            Console.Clear();
            Console.WriteLine("\n+++++++++++++++++++++++++++++++++++");
            Console.WriteLine($"Currently working in {stackname} stack\n");
            Console.WriteLine("To add or edit flashcards, go to \"Manage flashcards\" in the main menu.\n");
            Console.WriteLine("X to change current stack");
            Console.WriteLine("V to view all flashcards in this stack");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++\n");
            bool isValidInput = false;
            while (isValidInput == false)
            {
                string userInput = Console.ReadLine();
                switch (userInput.ToLower())
                {
                    case "0":
                        isValidInput = true;
                        MenuBuilders.MainMenu();
                        break;
                    case "x":
                        isValidInput = true;
                        MenuBuilders.ManageStacksMenu();
                        break;
                    case "v":
                        isValidInput = true;
                        Flashcards.ViewAllFlashcards(stackname);
                        break;
                    default:
                        isValidInput = false;
                        Console.WriteLine("Invalid input.\n");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
