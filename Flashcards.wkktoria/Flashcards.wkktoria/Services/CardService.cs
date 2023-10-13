using System.Data;
using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.UserInteractions;
using Microsoft.Data.SqlClient;

namespace Flashcards.wkktoria.Services;

internal class CardService
{
    private readonly SqlConnection _connection;
    private readonly string _databaseName;

    internal CardService(string connectionString, string databaseName)
    {
        _connection = new SqlConnection(connectionString);
        _databaseName = databaseName;
    }

    internal List<CardDto> GetAll(int stackId)
    {
        var cards = new List<CardDto>();

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT Front, Back FROM Cards WHERE StackId = {stackId};
                         """;
            var command = new SqlCommand(query, _connection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    cards.Add(new CardDto
                    {
                        Front = reader.GetString(0),
                        Back = reader.GetString(1)
                    });
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to get cards.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return cards;
    }

    internal Card GetByFront(string front, int stackId)
    {
        var card = new Card();

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT Id, StackId, Front, Back FROM Cards WHERE StackId = {stackId} AND Front = N'{front}';
                         """;
            var command = new SqlCommand(query, _connection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    card.Id = reader.GetInt32(0);
                    card.StackId = reader.GetInt32(1);
                    card.Front = reader.GetString(2);
                    card.Back = reader.GetString(3);
                }
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to get card.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return card;
    }

    internal bool Create(CardDto card, int stackId)
    {
        var created = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         INSERT INTO Cards(StackId, Front, Back)  VALUES({stackId}, N'{card.Front}', N'{card.Back}');
                         """;
            var command = new SqlCommand(query, _connection);

            created = command.ExecuteNonQuery() == 1;
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to create new card.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return created;
    }

    internal bool Delete(string front, int stackId)
    {
        var deleted = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         DELETE FROM Cards WHERE StackId = {stackId} AND Front = N'{front}';
                         """;
            var command = new SqlCommand(query, _connection);

            deleted = command.ExecuteNonQuery() == 1;
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to delete card.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return deleted;
    }

    internal bool Update(Card oldCard, CardDto newCard, int stackId)
    {
        var updated = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         UPDATE Cards SET Front = N'{newCard.Front}', Back = N'{newCard.Back}' WHERE Front = N'{oldCard.Front}' AND Back = N'{oldCard.Back}' AND StackId = {stackId};
                         """;
            var command = new SqlCommand(query, _connection);

            updated = command.ExecuteNonQuery() == 1;
        }
        catch (Exception)
        {
            UserOutput.ErrorMessage("Failed to update card.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return updated;
    }
}