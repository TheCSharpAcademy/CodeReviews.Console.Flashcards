using Dapper;
using Flashcards.Models.utils;
using Npgsql;

namespace Flashcards.Models;

public class MainRepository
{
    public readonly Seeder Seeder = new();
    public readonly StacksRepository Stacks = new();
    public readonly FlashcardsRepository Flashcards = new();
    public readonly SessionsRepository Sessions = new();

    private static void InitDatabase()
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.PostgresConnectionString))
            {
                connection.Open();

                var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{Context.DatabaseName}';";
                var dbCount = connection.ExecuteScalar<int>(sqlDbCount);

                if (dbCount == 0)
                {
                    var sql = $"CREATE DATABASE \"{Context.DatabaseName}\"";
                    connection.Execute(sql);
                }
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void InitTables()
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                NpgsqlCommand tableCommand =
                    new NpgsqlCommand(
                        @$"CREATE TABLE IF NOT EXISTS {Context.StacksTable} (
                                id              SERIAL PRIMARY KEY,
                                name            TEXT
                             );
                           CREATE TABLE IF NOT EXISTS {Context.FlashcardsTable} (
                                id              SERIAL PRIMARY KEY,
                                stack_id         INTEGER,
                                front_text       TEXT,
                                back_text        TEXT
                              );
                           CREATE TABLE IF NOT EXISTS {Context.SessionsTable} (
                                id              SERIAL PRIMARY KEY,
                                stack_id        INTEGER,
                                occurence_date  DATE,
                                score           INTEGER
                              );
                           ", connection);

                tableCommand.ExecuteNonQuery();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void Init()
    {
        InitDatabase();
        InitTables();
    }

    public void DeleteData()
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                NpgsqlCommand tableCommand =
                    new NpgsqlCommand(
                        @$"DROP TABLE {Context.StacksTable};
                           DROP TABLE {Context.FlashcardsTable}; 
                           DROP TABLE {Context.SessionsTable};
                           ", connection);

                tableCommand.ExecuteNonQuery();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }
}