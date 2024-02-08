using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Flashcards.jkjones98;

internal class StudySessionController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    internal void StartStudySession()
    {
        
    }

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
                        Date = reader.GetString(1),
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

    internal void StudySessionsYear(int stackId, string date)
    {
        List<StudySession> studySessions = new List<StudySession>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM StudySessions WHERE Date like '%{date}%' AND StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                studySessions.Add(new StudySession
                    {
                        StudyId = reader.GetInt32(0), 
                        Date = reader.GetString(1),
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
}