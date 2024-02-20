using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleTableExt;
using FlashCards.Ibrahim.Models;

namespace FlashCards.Ibrahim.Database_Access
{
    public class StudySession_DB_Access
    {
        static string _connectionString;
        public StudySession_DB_Access(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void InsertStudySession(int Stacks_Id, DateTime date,int score)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"
                        INSERT INTO StudySession_Table (Stacks_Id,Score,Date) 
                        VALUES (@Stacks_Id,@Score,@Date)";
                command.Parameters.AddWithValue("@Stacks_Id", Stacks_Id);
                command.Parameters.AddWithValue("@Score", score);
                command.Parameters.AddWithValue("@Date", date);
                command.ExecuteNonQuery();
            }
        }

        public static List<StudySession> GetAllSessions()
        {
            List<StudySession> studySessions = new List<StudySession>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM StudySession_Table";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudySession session = new StudySession();
                        session.Id = reader.GetInt32(0);
                        session.Stacks_Id = reader.GetInt32(1);
                        session.Date = reader.GetDateTime(3); 
                        session.Score = reader.GetInt32(2);
                        studySessions.Add(session);
                    }
                }
            }
            return studySessions;
        }
    
        public static void GetReports(int year,int stackId)
        {
            List<Report> avgReport = new List<Report>();
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = @$"DECLARE @Year int = @inputYear
                                         DECLARE @Stack int = @inputStack

SELECT
    ISNULL([1], 0) AS January,
    ISNULL([2], 0) AS February,
    ISNULL([3], 0) AS March,
    ISNULL([4], 0) AS April,
    ISNULL([5], 0) AS May,
    ISNULL([6], 0) AS June,
    ISNULL([7], 0) AS July,
    ISNULL([8], 0) AS August,
    ISNULL([9], 0) AS September,
    ISNULL([10], 0) AS October,
    ISNULL([11], 0) AS November,
    ISNULL([12], 0) AS December
FROM
    (SELECT
        score,
        MONTH(date) AS [month]
    FROM
        StudySession_Table
    WHERE
        YEAR(date) = @Year AND Stacks_Id = @Stack
) AS SourceTable
PIVOT
    (
    AVG(score)
    FOR [month] IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
    ) AS PivotTable;
";
                command.Parameters.AddWithValue("@inputYear", year);
                command.Parameters.AddWithValue("@inputStack", stackId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    Console.Clear();
                    Console.WriteLine("Average score per month \n");
                    ConsoleTableBuilder
                        .From(dataTable)
                        .ExportAndWriteLine();
                }
            }

        }
    }
}
