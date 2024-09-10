using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Flashcards;


namespace Flashcards
{
    public class ReportApp
    {
        string connectionString = "Server=localhost;Database=FLASHCARDS;User ID=SA;Password=Password123;";

        public void ShowYearlyReport()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Input a year in format YYYY");
            Console.WriteLine("----------------------------");

            int year = Convert.ToInt32(Console.ReadLine());
            GenerateYearlyReport(year);
        }

        private void GenerateYearlyReport(int year)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string query = @"
            SELECT s.StackName, 
                   MONTH(ss.[Date]) AS Month, 
                   SUM(ss.Score) AS TotalScore
            FROM StudySession ss
            INNER JOIN Stacks s ON ss.StackID = s.StackID
            WHERE YEAR(ss.[Date]) = @Year
            GROUP BY s.StackName, MONTH(ss.[Date])
            ORDER BY s.StackName, Month";

        var sessions = connection.Query(query, new { Year = year }).ToList();

        if (sessions.Count == 0)
        {
            Console.WriteLine($"No records found for the year {year}.");
            return;
        }

        
        Console.WriteLine("----------------------------+------------------- Average per month for: {0} -------------------+", year);
        Console.WriteLine("| StackName  | January | February | March | April | May | June | July | August | September | October | November | December |");
        Console.WriteLine("+------------+---------+----------+-------+-------+-----+------+------+--------+-----------+---------+----------+----------+");

        
        var stackReports = new Dictionary<string, int[]>();

        foreach (var row in sessions)
        {
            string stackName = row.StackName;
            int month = row.Month;
            int score = row.TotalScore;

            if (!stackReports.ContainsKey(stackName))
            {
                stackReports[stackName] = new int[12]; 
            }

            stackReports[stackName][month - 1] = score; 
        }

        foreach (var stackReport in stackReports)
        {
            string stackName = stackReport.Key;
            int[] monthlyScores = stackReport.Value;

            Console.Write($"| {stackName} | ");
            for (int i = 0; i < 12; i++)
            {
                Console.Write($"{monthlyScores[i].ToString()} | ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("+------------+---------+----------+-------+-------+-----+------+------+--------+-----------+---------+----------+----------+");

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }
}

    }
}
