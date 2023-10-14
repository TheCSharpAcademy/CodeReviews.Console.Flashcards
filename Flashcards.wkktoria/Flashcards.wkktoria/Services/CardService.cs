using System.Data;
using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services.Helpers;
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
                         USE {_databaseName}

                         SELECT Front, Back FROM Cards WHERE StackId = @stackId
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@stackId", stackId);

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

        return CardHelper.ToFullDto(cards);
    }

    internal Card? GetByDtoId(int dtoId, int stackId)
    {
        var cards = GetAll(stackId);
        var card = cards.Find(card => card.DtoId == dtoId);

        return card == null ? null : GetByFront(card.Front, stackId);
    }

    private Card GetByFront(string front, int stackId)
    {
        var card = new Card();

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName}

                         SELECT Id, StackId, Front, Back FROM Cards WHERE StackId = @stackId AND Front = @front
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@stackId", stackId);
            command.Parameters.AddWithValue("@front", front);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    card.Id = reader.GetInt32(0);
                    reader.GetInt32(1);
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
                         USE {_databaseName}

                         INSERT INTO Cards(StackId, Front, Back) VALUES(@stackId, @front, @back)
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@stackId", stackId);
            command.Parameters.AddWithValue("@front", card.Front);
            command.Parameters.AddWithValue("@back", card.Back);

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

    internal bool Delete(int cardId, int stackId)
    {
        var deleted = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName}

                         DELETE FROM Cards WHERE Id = @cardId AND StackId = @stackId
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@cardId", cardId);
            command.Parameters.AddWithValue("@stackId", stackId);

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
                         USE {_databaseName}

                         UPDATE Cards SET Front = @newFront, Back = @newBack WHERE Front = @oldFront AND Back = @oldBack AND StackId = @stackId
                         """;
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@newFront", newCard.Front);
            command.Parameters.AddWithValue("@newBack", newCard.Back);
            command.Parameters.AddWithValue("@oldFront", oldCard.Front);
            command.Parameters.AddWithValue("@oldBack", oldCard.Back);
            command.Parameters.AddWithValue("@stackId", stackId);

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