using System.Data;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.UserInteractions;
using Microsoft.Data.SqlClient;

namespace Flashcards.wkktoria.Services;

internal class SessionService
{
    private readonly SqlConnection _connection;
    private readonly string _databaseName;

    internal SessionService(string connectionString, string databaseName)
    {
        _connection = new SqlConnection(connectionString);
        _databaseName = databaseName;
    }

    internal List<SessionDto> GetAll(int stackId)
    {
        var sessions = new List<SessionDto>();

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName}

                         SELECT FinishedDate, Score FROM Sessions WHERE StackId = @stackId
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@stackId", stackId);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    sessions.Add(new SessionDto
                    {
                        FinishedDate = reader.GetDateTime(0),
                        Score = reader.GetInt32(1)
                    });
        }
        catch (Exception ex)
        {
            UserOutput.ErrorMessage(ex.Message);
            UserOutput.ErrorMessage("Failed to get sessions.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return sessions;
    }

    internal bool Create(SessionDto session, int stackId)
    {
        var created = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         INSERT INTO Sessions(StackId, FinishedDate, Score) VALUES(@stackId, @finishedDate, @score)
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@stackId", stackId);
            command.Parameters.AddWithValue("@finishedDate", session.FinishedDate);
            command.Parameters.AddWithValue("@score", session.Score);

            created = command.ExecuteNonQuery() == 1;
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to create new session.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return created;
    }
}