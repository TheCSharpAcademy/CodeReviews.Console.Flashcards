using Dapper;
using Flashcards.Models.Dtos;
using Flashcards.Models.utils;
using Npgsql;

namespace Flashcards.Models;

public class StacksRepository
{
    public void DeleteById(int id)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"DELETE FROM {Context.StacksTable} WHERE id=@Id";

                connection.Execute(tableCommand, new { Id = id });
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void Update(StackDto stack)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"UPDATE {Context.StacksTable} SET name=@Name WHERE id=@Id";

                connection.Execute(tableCommand, stack);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void Insert(StackDto stack)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"INSERT INTO {Context.StacksTable} (name)
                       VALUES (@Name)";

                connection.Execute(tableCommand, stack);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public List<StackDto> GetAll()
    {
        List<StackDto> stacks = [];
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                stacks = connection.Query<StackDto>(@$"SELECT Id, Name FROM {Context.StacksTable} 
                                                   ORDER BY id")
                    .ToList();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }

        return stacks;
    }
}