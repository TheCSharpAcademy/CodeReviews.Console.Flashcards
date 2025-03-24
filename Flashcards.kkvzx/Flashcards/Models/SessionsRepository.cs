using Dapper;
using Flashcards.Models.Dtos;
using Flashcards.Models.utils;
using Npgsql;

namespace Flashcards.Models;

public class SessionsRepository
{
    public void DeleteByStackId(int stackId)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"DELETE FROM {Context.SessionsTable}
                       WHERE stack_id=@StackId";

                connection.Execute(tableCommand, new { StackId = stackId });
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void Insert(SessionDto session)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"INSERT INTO {Context.SessionsTable} (stack_id, occurence_date, score )
                       VALUES (@StackId, @OccurenceDate, @Score)";

                connection.Execute(tableCommand, session);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public List<SessionDto> GetAll()
    {
        List<SessionDto> sessions = [];

        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                sessions = connection
                    .Query<SessionDto>(
                        @$"SELECT Id, stack_id AS StackId, occurence_date AS OccurenceDate, score FROM {Context.SessionsTable}
                           ORDER BY id")
                    .ToList();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }

        return sessions;
    }
}