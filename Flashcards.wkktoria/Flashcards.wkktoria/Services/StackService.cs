using System.Data;
using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services.Helpers;
using Microsoft.Data.SqlClient;

namespace Flashcards.wkktoria.Services;

internal class StackService
{
    private readonly SqlConnection _connection;
    private readonly string _databaseName;

    internal StackService(string connectionString, string databaseName)
    {
        _connection = new SqlConnection(connectionString);
        _databaseName = databaseName;
    }

    internal List<StackDto> GetAll()
    {
        var stacks = new List<StackDto>();

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT Name FROM Stacks;
                         """;
            var command = new SqlCommand(query, _connection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                    stacks.Add(new StackDto
                    {
                        Name = reader.GetString(0)
                    });
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to get all stacks.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return StackHelper.ToFullDto(stacks);
    }

    internal Stack GetByName(string name)
    {
        var stack = new Stack();

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT Id, Name FROM Stacks WHERE Name = N'{name}';
                         """;
            var command = new SqlCommand(query, _connection);

            var reader = command.ExecuteReader();

            if (reader.HasRows)
                while (reader.Read())
                {
                    stack.Id = reader.GetInt32(0);
                    stack.Name = reader.GetString(1);
                }
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to get stack.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return stack;
    }

    internal bool CheckIfNameExists(string name)
    {
        var nameExists = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         SELECT * FROM Stacks WHERE Name = N'{name}';
                         """;
            var command = new SqlCommand(query, _connection);
            var reader = command.ExecuteReader();

            nameExists = reader.HasRows;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to check if name exists.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return nameExists;
    }

    internal bool Create(StackDto stack)
    {
        var created = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         INSERT INTO Stacks(Name)  VALUES(N'{stack.Name}');
                         """;
            var command = new SqlCommand(query, _connection);

            created = command.ExecuteNonQuery() == 1;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to create new stack.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return created;
    }

    internal bool Delete(int id)
    {
        var deleted = false;

        try
        {
            _connection.Open();

            var query = $"""
                         USE {_databaseName};

                         DELETE FROM Stacks WHERE Id = {id};
                         """;
            var command = new SqlCommand(query, _connection);

            deleted = command.ExecuteNonQuery() == 1;
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to delete stack.");
        }
        finally
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        return deleted;
    }
}