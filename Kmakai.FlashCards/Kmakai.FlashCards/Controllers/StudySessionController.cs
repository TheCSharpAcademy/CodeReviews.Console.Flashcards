using Kmakai.FlashCards.Models;
using System.Data.SqlClient;
using System.Configuration;
using ConsoleTableExt;

namespace Kmakai.FlashCards.Controllers;

public class StudySessionController
{
    private static string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    public static void AddSession(StudySession session)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                INSERT INTO StudySessions (StackId, Score, Date)
                VALUES ('{session.StackId}', '{session.Score}', '{session.Date}')";
            command.ExecuteNonQuery();
            connection.Close();
        }

    }

    public static List<StudySession> GetSessions()
    {
        List<StudySession> sessions = new List<StudySession>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT * FROM StudySessions";

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                StudySession session = new StudySession();
                session.Id = reader.GetInt32(0);
                session.StackId = reader.GetInt32(1);
                session.Score = reader.GetInt32(2);
                session.Date = DateOnly.FromDateTime(reader.GetDateTime(3));
                sessions.Add(session);
            }

            connection.Close();
        }

        return sessions;
    }

    public static StudySession CreateStudySession(Stack stack, List<Flashcard> cards)
    {
        int score = 0;
        StudySession session = new StudySession();
        session.StackId = stack.Id;
        session.Date = DateOnly.FromDateTime(DateTime.Now);

        foreach (Flashcard card in cards)
        {
            Console.Clear();
            var table = new List<List<object?>> { new List<object?> { card.Front } };
            ConsoleTableBuilder.From(table).WithFormat(ConsoleTableBuilderFormat.Alternative).WithColumn("Front").ExportAndWriteLine();

            Console.WriteLine("What is the answer for this card");
            Console.Write("Answer: ");
            string? answer = Console.ReadLine();
            if (answer == card.Back)
            {
                table = new List<List<object?>> { new List<object?> { card.Front, card.Back } };
                ConsoleTableBuilder.From(table).WithColumn("Front", "back").ExportAndWriteLine();
                Console.WriteLine("Correct!");
                score++;

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Incorrect. The answer is {card.Back}");
                table = new List<List<object?>> { new List<object?> { card.Front, card.Back } };
                ConsoleTableBuilder.From(table).WithFormat(ConsoleTableBuilderFormat.Alternative).WithColumn("Front", "back").ExportAndWriteLine();

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        session.Score = score;
        AddSession(session);
        return session;
    }
}
