using System.Xml;
using Flashcards.glaxxie.Utilities;
using Microsoft.Data.SqlClient;

namespace Flashcards.glaxxie.Controllers;

internal static class Tables
{
    internal static readonly string Sessions = AppConfiguration.Instance.SessionsTable;
    internal static readonly string Stacks = AppConfiguration.Instance.StacksTable;
    internal static readonly string Cards = AppConfiguration.Instance.CardsTable;
}

internal sealed class DatabaseController
{
    private static DatabaseController? _instance;
    internal static DatabaseController Instance
    {
        get
        {
            _instance ??= new DatabaseController();
            return _instance;
        }
    }

    internal static SqlConnection GetConnection () => new(AppConfiguration.Instance.ConnectionString);
    private DatabaseController()
    {
        SetupTables();
    }

    private static void SetupTables()
    {
        string[] initCmds = [
            $@"IF OBJECT_ID('dbo.{Tables.Stacks}', 'U') IS NULL
            BEGIN
                CREATE TABLE dbo.{Tables.Stacks} (
                    stack_id INT PRIMARY KEY IDENTITY(1,1),
                    stack_name NVARCHAR(100) UNIQUE
            )
            END
            ",

            $@"IF OBJECT_ID('dbo.{Tables.Cards}', 'U') IS NULL
            BEGIN
                CREATE TABLE dbo.{Tables.Cards} (
                    card_id INT PRIMARY KEY IDENTITY(1,1),
                    front NVARCHAR(MAX),
                    back NVARCHAR(MAX),
                    stack_id INT,
                    FOREIGN KEY (stack_id) REFERENCES dbo.{Tables.Stacks}(stack_id)
                )
            END",

            $@"IF OBJECT_ID('dbo.{Tables.Sessions}', 'U') IS NULL
            BEGIN
                CREATE TABLE dbo.{Tables.Sessions} (
                    session_id INT PRIMARY KEY IDENTITY(1,1),
                    date DATETIME,
                    score INT,
                    stack_id INT,
                    FOREIGN KEY (stack_id) REFERENCES dbo.{Tables.Stacks}(stack_id)
                )
            END"

           ];
        ExecuteCommand(initCmds);
    }

    internal static void ExecuteCommand(IEnumerable<string> command)
    {
        using var conn = GetConnection();
        conn.Open();
        foreach (var cmd in command)
        {
            SqlCommand query = new(cmd, conn);
            query.ExecuteNonQuery();
        }
    }
}


           //@$"CREATE TABLE IF NOT EXISTS {Tables.Stacks} (
           //         stack_id INTEGER PRIMARY KEY AUTOINCREMENT,
           //         stack_name TEXT UNIQUE
           //         )",


           // @$"CREATE TABLE IF NOT EXISTS {Tables.Cards} (
           //         card_id INTEGER PRIMARY KEY AUTOINCREMENT,
           //         front TEXT,
           //         back TEXT,
           //         FOREIGN KEY (stack_id) REFERENCES {Tables.Stacks}(stack_id)
           //         )",

           // @$"CREATE TABLE IF NOT EXISTS {Tables.Sessions} (
           //         session_id INTEGER PRIMARY KEY AUTOINCREMENT,
           //         date TEXT,
           //         score INT,
           //         FOREIGN KEY (stack_id) REFERENCES {Tables.Stacks}(stack_id)
           //         )"