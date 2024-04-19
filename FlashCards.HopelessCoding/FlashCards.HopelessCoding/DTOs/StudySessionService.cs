using Spectre.Console;
using System.Data.SqlClient;

namespace FlashCards.HopelessCoding.DTOs;
    internal class StudySessionService
    {

    private readonly string _connectionString;

    public StudySessionService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<StudySessionDTO> GetStudySession()
    {
        List<StudySessionDTO> studySessions = new List<StudySessionDTO>();

        string query = "SELECT * FROM StudySessions;";

        using (var connection = new SqlConnection(_connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudySessionDTO session = new StudySessionDTO
                        {
                            Stack = reader.GetString(1),
                            Date = reader.GetDateTime(2).Date,
                            Score = reader.GetInt32(3)
                        };

                        studySessions.Add(session);
                    }
                }
            }
        }
        return studySessions;
    }

    public void PrintAllStudySessions()
    {
        Console.Clear();
        List<StudySessionDTO> studySessions = GetStudySession();

        if (studySessions.Count > 0)
        {
            var studySessionsTable = new Table();
            studySessionsTable.Title = new TableTitle($"[yellow1]All study sessions[/]");
            studySessionsTable.Border = TableBorder.Rounded;
            studySessionsTable.AddColumn("[gold1]Stack[/]");
            studySessionsTable.AddColumn("[gold1]Date[/]");
            studySessionsTable.AddColumn("[gold1]Score[/]");
            studySessionsTable.Columns[0].Padding(1, 0);

            foreach (var session in studySessions)
            {
                studySessionsTable.AddRow($"{session.Stack}", $"{session.Date.ToString("dddd dd.MM.yyyy")}", $"{session.Score}");
            }

            AnsiConsole.Write(studySessionsTable);
        }
        else
        {
            Console.WriteLine("No study sessions found.");
        }
    }
}
