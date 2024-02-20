using System.Configuration;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Flashcards.jkjones98;

internal class StudySessionController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal List<FlashcardDto> GetFlashcards(int stackId)
    {
        List<FlashcardDto> flashcardTable = new List<FlashcardDto>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM Flashcards WHERE StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                flashcardTable.Add(new FlashcardDto
                    {
                        FlashcardId = reader.GetInt32(0), 
                        Front = reader.GetString(1),
                        Back = reader.GetString(2),
                    });
            }
        }

        return flashcardTable;
    }

    internal void InsertStudyDb(StudySession studySession)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $@"INSERT INTO StudySessions (Date, Score, Studied, Language, StackId) 
            VALUES ('{studySession.Date}', {studySession.Score}, {studySession.Studied}, '{studySession.Language}', {studySession.StackId})";
        tableCmd.ExecuteNonQuery();
    }

    internal string GetLanguageName(int stackId)
    {
        string language = "Unable to retrieve language";
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM Stacks WHERE StackId = {stackId}";
        var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                language = reader.GetString(1);
            }
        }

        return language;
    }

    internal void ViewStudySessions(int stackId)
    {
        List<StudySession> studySessions = new List<StudySession>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM StudySessions WHERE StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                studySessions.Add(new StudySession
                    {
                        StudyId = reader.GetInt32(0), 
                        Date = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                        Score = reader.GetInt32(2),
                        Studied = reader.GetInt32(3),
                        Language = reader.GetString(4),
                        StackId = reader.GetInt32(5)
                    });
            }
        }
        else
            Console.WriteLine("No rows found");


        ShowTable.CreateSessionTable(studySessions);
    }

    internal void StudySessionsYear(int stackId, int date)
    {
        List<StudySession> studySessions = new List<StudySession>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM StudySessions WHERE YEAR(Date) = {date} AND StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                studySessions.Add(new StudySession
                    {
                        StudyId = reader.GetInt32(0), 
                        Date = reader.GetDateTime(1).Date.ToString("dd-MM-yyyy"),
                        Score = reader.GetInt32(2),
                        Studied = reader.GetInt32(3),
                        Language = reader.GetString(4),
                        StackId = reader.GetInt32(5)
                    });
            }
        }
        else
            Console.WriteLine("No rows found");


        ShowTable.CreateSessionTable(studySessions);
    }

    internal void StudySessionsMonth(int stackId, int month, int year)
    {
        List<StudySession> studySessions = new List<StudySession>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM StudySessions WHERE MONTH(Date) = {month} AND YEAR(Date) = {year} AND StackId = {stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                studySessions.Add(new StudySession
                    {
                        StudyId = reader.GetInt32(0), 
                        Date = reader.GetDateTime(1).Date.ToString("dd-MM-yyyy"),
                        Score = reader.GetInt32(2),
                        Studied = reader.GetInt32(3),
                        Language = reader.GetString(4),
                        StackId = reader.GetInt32(5)
                    });
            }
        }
        else
            Console.WriteLine("No rows found");


        ShowTable.CreateSessionTable(studySessions);
    }

    internal void PivotMonthlySessions(int stackId, int year)
    {
        List<PivotCountMonth> studySessions = new List<PivotCountMonth>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $@"SELECT 
            CAST(Language AS varchar(MAX)) AS Language,
            ISNULL([1],0) AS January,
            ISNULL([2],0) AS February,
            ISNULL([3],0) AS March,
            ISNULL([4],0) AS April,
            ISNULL([5],0) AS May,
            ISNULL([6],0) AS June,
            ISNULL([7],0) AS July,
            ISNULL([8],0) AS August,
            ISNULL([9],0) AS September,
            ISNULL([10],0) AS October,
            ISNULL([11],0) AS November,
            ISNULL([12],0) AS December
            FROM 
            (SELECT CAST(Language AS varchar(MAX)) AS Language, Studied, MONTH(Date) AS TMonth
                FROM StudySessions WHERE YEAR(Date) = {year} AND StackID= {stackId}) AS SourceTable
            PIVOT 
            (
                COUNT(Studied)
                FOR TMonth
                IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
            ) AS PivotTable";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                studySessions.Add(new PivotCountMonth
                    {
                        Language = reader.GetString(0),
                        January = reader.GetInt32(1),
                        February = reader.GetInt32(2),
                        March = reader.GetInt32(3),
                        April = reader.GetInt32(4),
                        May = reader.GetInt32(5),
                        June = reader.GetInt32(6),
                        July = reader.GetInt32(7),
                        August = reader.GetInt32(8),
                        September = reader.GetInt32(9),
                        October= reader.GetInt32(10),
                        November = reader.GetInt32(11),
                        December = reader.GetInt32(12),
                    });
            }
        }
        else
            Console.WriteLine("No rows found");


        ShowTable.CreateStudyTable(studySessions);
    }

    internal void PivotMonthlyAverages(int stackId, int year)
    {
        List<PivotCountMonth> studySessions = new List<PivotCountMonth>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $@"SELECT 
            CAST(Language AS varchar(MAX)) AS Language,
            ISNULL([1],0) AS January,
            ISNULL([2],0) AS February,
            ISNULL([3],0) AS March,
            ISNULL([4],0) AS April,
            ISNULL([5],0) AS May,
            ISNULL([6],0) AS June,
            ISNULL([7],0) AS July,
            ISNULL([8],0) AS August,
            ISNULL([9],0) AS September,
            ISNULL([10],0) AS October,
            ISNULL([11],0) AS November,
            ISNULL([12],0)AS December
            FROM 
            (SELECT CAST(Language AS varchar(MAX)) AS Language, Score, MONTH(Date) AS TMonth
                FROM StudySessions WHERE YEAR(Date) = {year} AND StackId = {stackId} AS SourceTable
            PIVOT 
            (
                AVG(Score)
                FOR TMonth
                IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
            ) AS PivotTable";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                studySessions.Add(new PivotCountMonth
                    {
                        Language = reader.GetString(0),
                        January = reader.GetInt32(1),
                        February = reader.GetInt32(2),
                        March = reader.GetInt32(3),
                        April = reader.GetInt32(4),
                        May = reader.GetInt32(5),
                        June = reader.GetInt32(6),
                        July = reader.GetInt32(7),
                        August = reader.GetInt32(8),
                        September = reader.GetInt32(9),
                        October= reader.GetInt32(10),
                        November = reader.GetInt32(11),
                        December = reader.GetInt32(12),
                    });
            }
        }
        else
            Console.WriteLine("No rows found");


        ShowTable.CreateStudyTable(studySessions);
    }
}