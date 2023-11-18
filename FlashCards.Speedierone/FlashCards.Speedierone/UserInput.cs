using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FlashCards;

internal class UserInput
{
    internal static void ViewStacks()
    {
        Console.Clear();
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM Stack";

            List<StudySessions> tableData = new();

            SqlDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new StudySessions
                    {
                        Subject = reader.GetString(0),
                    });
                }
                TableLayout.DisplayTableStack(tableData);
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();
        }
    }
    internal static void ViewFlashCards()
    {
        Console.Clear();
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM Questions";

            List<StudySessions> tableData = new();

            SqlDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new StudySessions
                    {
                        ID = reader.GetInt32(0),
                        FlashSubject = reader.GetString(1),
                        FrontCard = reader.GetString(2),
                        BackCard = reader.GetString(3),
                    });
                }
                TableLayout.DisplayTableFlashCard(tableData);
            }
            else
            {
                Console.WriteLine("No rows found. Press enter to continue.");
                Console.ReadLine();
                MainMenu.ShowMenu();
            }
            connection.Close();
        }
        Console.WriteLine("Press any button to continue.");
        Console.ReadLine();
    }
    internal static void AddStack()
    {
        Console.Clear();
        Console.WriteLine("What would you like to call the new subject?");
        var subject = Console.ReadLine();
        Helpers.CheckSubjectDuplicate(subject);
        try
        {
            var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                   $"INSERT INTO Stack (Subject) Values ('{subject}')";
                tableCmd.ExecuteNonQuery();            
                connection.Close();
            }
            Console.WriteLine($"{subject} added. Press any key to return to Main Menu.");
            Console.ReadLine();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"{ex.Message} Press any button to continue.");
            Console.ReadLine();
            AddStack();
        }
    }
    internal static void AddFlashCard()
    {
        Console.Clear();
        ViewStacks();
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        Console.WriteLine("Please enter subject you would like to add a card to.");
        var subject = Console.ReadLine();
        Helpers.CheckSubjectExists(subject);
        Console.WriteLine("How many questions would you like to add?");
        var amount = int.Parse(Console.ReadLine());
        Console.Clear();
        

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                for (int i = 0; i < amount; i++)
                {
                    Console.Clear();
                    Console.WriteLine("Please enter flashcard question.");
                    var question = Console.ReadLine();
                    Console.WriteLine("Please enter answer.");
                    var answer = Console.ReadLine();
                    var tableCmd = connection.CreateCommand();
                    var newID = Helpers.CreateCustomId();
                    tableCmd.CommandText =
                        $"INSERT INTO Questions (FlashSubject, FrontOfCard, BackOfCard, ID) VALUES ('{subject}', '{question}', '{answer}', {newID})";
                    tableCmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            Console.WriteLine($"New flashcard added successfully into {subject}. Press any key to return to main menu.");
            Console.ReadLine();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"{ex.Message}. Press enter to continue.");
            Console.ReadLine();
        }
    }
    internal static void DeleteStack()
    {
        ViewStacks();

        Console.WriteLine("Please enter name of stack you wish to delete.");
        var subject = Console.ReadLine();
        if (subject == "0") MainMenu.ShowMenu();

        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"DELETE FROM Stack WHERE Subject = '{subject}'";
            var rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"Record with {subject} does not exist. Press any button to continue.");
                Console.ReadLine();
                DeleteStack();
            }
            connection.Close();
        }
        Console.WriteLine($"{subject} deleted from stacks. Press any key to continue.");
        Console.ReadLine();
    }
    internal static void DeleteFlashCard()
    {
        ViewFlashCards();

        Console.WriteLine("Please choose ID of flashcard you would like to delete.");
        var ID = int.Parse(Console.ReadLine());

        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"DELETE FROM Questions WHERE ID = {ID}";
            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"Record with {ID} does not exist. Press any button to continue.");
                Console.ReadLine();
                DeleteFlashCard();
            }
        }
        Console.WriteLine($"{ID} deleted from stacks. Press any key to continue.");
        Console.ReadLine();
    }
    internal static void ShowGameHistory()
    {
        Console.Clear();
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM StudyGames";
            List<StudySessions> tableData = new();
            SqlDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new StudySessions
                    {
                        Date = reader.GetDateTime(0),
                        Subject = reader.GetString(1),
                        GameScore = reader.GetDouble(2),
                        GameAmount = reader.GetInt32(3),
                    });
                }
                TableLayout.DisplayGameHistory(tableData);
            }
            else
            {
                Console.WriteLine("No game history to show.");

            }
            connection.Close();
            Console.WriteLine("Press any button to continue.");
            Console.ReadLine();
        }
    }
    internal static void TotalSessionsMonth()
    {
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"WITH Months AS(
                SELECT 1 AS MonthNum, 'January' AS MonthName
                UNION SELECT 2, 'February'
                UNION SELECT 3, 'March'
                UNION SELECT 4, 'April'
                UNION SELECT 5, 'May'
                UNION SELECT 6, 'June'
                UNION SELECT 7, 'July'
                UNION SELECT 8, 'August'
                UNION SELECT 9, 'September'
                UNION SELECT 10, 'October'
                UNION SELECT 11, 'November'
                UNION SELECT 12, 'December'
                )
            SELECT*
            FROM
            (
                SELECT 'GameAmount' AS Category, Months.MonthName, GameAmount
                FROM Months
                LEFT JOIN StudyGames ON Months.MonthNum = MONTH(StudyGames.Date)
            )AS SourceTable
            PIVOT
            (
                COUNT(GameAmount)
                FOR MonthName IN([January], [February], [March], [April], [May], [June],
                   [July], [August], [September], [October], [November], [December]
            )) AS PivotTable";

            var adapter = new SqlDataAdapter(tableCmd);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            TableLayout.DisplayScoreMonthly(dataTable);
        }
    }
    internal static void AverageScoreMonth()
    {
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"WITH Months AS(
            SELECT 1 AS MonthNum, 'January' AS MonthName
            UNION SELECT 2, 'February'
            UNION SELECT 3, 'March'
            UNION SELECT 4, 'April'
            UNION SELECT 5, 'May'
            UNION SELECT 6, 'June'
            UNION SELECT 7, 'July'
            UNION SELECT 8, 'August'
            UNION SELECT 9, 'September'
            UNION SELECT 10, 'October'
            UNION SELECT 11, 'November'
            UNION SELECT 12, 'December'
            )
        SELECT*
        FROM
        (
            SELECT 'GameScore' AS Category, Months.MonthName, GameScore
            FROM Months
            LEFT JOIN StudyGames ON Months.MonthNum = MONTH(StudyGames.Date)
        )AS SourceTable
        PIVOT
        (
            AVG(GameScore)
            FOR MonthName IN([January], [February], [March], [April], [May], [June],
               [July], [August], [September], [October], [November], [December]
        )) AS PivotTable";

            var adapter = new SqlDataAdapter(tableCmd);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            TableLayout.DisplayAverageScore(dataTable);
        }
    }
}