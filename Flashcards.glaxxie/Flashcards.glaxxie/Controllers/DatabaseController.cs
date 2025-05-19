using Flashcards.glaxxie.Utilities;
using Microsoft.Data.SqlClient;

namespace Flashcards.glaxxie.Controllers;

internal static class Tables
{
    internal static readonly string Sessions = AppSettings.SessionsTable;
    internal static readonly string Stacks = AppSettings.StacksTable;
    internal static readonly string Cards = AppSettings.CardsTable;
}

internal static class DatabaseController
{
    internal static void Setup()
    {
        SetupDatabase();
        SetupTables();
    }

    internal static SqlConnection GetConnection(string? connStr = null) =>
        new(string.IsNullOrWhiteSpace(connStr) ? AppSettings.DefaultConnectionString : connStr);

    private static void SetupDatabase()
    {
        var cmd = @"
            IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FlashcardsDb')
            BEGIN
                CREATE DATABASE FlashcardsDb
            END";
        ExecuteCommand(AppSettings.MasterConnectionString, cmd);
    }

    private static void SetupTables()
    {
        string[] initCmds = [
            $@"IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = '{Tables.Stacks}')
            BEGIN
                CREATE TABLE {Tables.Stacks} (
                    stack_id INT PRIMARY KEY IDENTITY(1,1),
                    stack_name NVARCHAR(100) UNIQUE
            )
            END
            ",

            $@"IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = '{Tables.Cards}')
            BEGIN
                CREATE TABLE {Tables.Cards} (
                    card_id INT PRIMARY KEY IDENTITY(1,1),
                    front NVARCHAR(255) UNIQUE,
                    back NVARCHAR(255),
                    stack_id INT,
                    FOREIGN KEY (stack_id) REFERENCES {Tables.Stacks}(stack_id) ON DELETE CASCADE
                )
            END",

            $@"IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = '{Tables.Sessions}')
            BEGIN
                CREATE TABLE {Tables.Sessions} (
                    session_id INT PRIMARY KEY IDENTITY(1,1),
                    date DATETIME,
                    score INT,
                    cards INT,
                    stack_id INT,
                    FOREIGN KEY (stack_id) REFERENCES {Tables.Stacks}(stack_id) ON DELETE CASCADE
                )
            END"

           ];
        ExecuteCommand(AppSettings.DefaultConnectionString, initCmds);
    }

    internal static void ExecuteCommand(string connStr, params string[] commands)
    {
        using var conn = GetConnection(connStr);
        conn.Open();
        foreach (var cmd in commands)
        {
            SqlCommand query = new(cmd, conn);
            query.ExecuteNonQuery();
        }
    }
}