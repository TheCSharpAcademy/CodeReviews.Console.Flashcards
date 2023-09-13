namespace Flashcards;

using System.Data.SqlClient;

internal static class DatabaseSetup
{
    internal static bool CreateTablesIfNotPresent(string? databaseConnectionString)
    {
        try
        {
            Logger.Info($"Database Setup");
            if (String.IsNullOrEmpty(databaseConnectionString))
            {
                throw new InvalidOperationException("Database connection string is empty or not configured.");
            }
            using var connection = new SqlConnection(databaseConnectionString);
            Logger.Info($"Trying to connect to database '{connection.Database}'...");
            connection.Open();
            Logger.Info($"Connected to database '{connection.Database}'.");
            CreateTableStacks(connection);
            CreateTableFlashcards(connection);
            CreateTableStudySessions(connection);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

    private static bool TableExists(string tablename, SqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT object_id('{tablename}', 'U');";
        var objectId = command.ExecuteScalar();
        var tableExists = objectId != DBNull.Value;
        if (tableExists)
        {
            LogTableInfo(tablename, connection);
        }
        return tableExists;
    }

    private static void LogTableInfo(string tablename, SqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(*) FROM {tablename};";
        var rowCount = command.ExecuteScalar();
        Logger.Info($"Table '{tablename}' exists and contains {rowCount} rows.");
    }

    private static void CreateTableStacks(SqlConnection connection)
    {
        if (TableExists("stacks", connection))
        {
            return;
        }

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE [dbo].[stacks] (
            [id]   INT           IDENTITY (1, 1) NOT NULL,
            [name] NVARCHAR (50) NOT NULL,
            CONSTRAINT [PK_stacks] PRIMARY KEY CLUSTERED ([id] ASC)
        );
        ";
        command.ExecuteNonQuery();
        Logger.Info("Table 'stacks' created.");
    }

    private static void CreateTableFlashcards(SqlConnection connection)
    {
        if (TableExists("flashcards", connection))
        {
            return;
        }

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE [dbo].[flashcards] (
            [id]       INT           IDENTITY (1, 1) NOT NULL,
            [stack_id] INT           NOT NULL,
            [front]    NVARCHAR (50) NOT NULL,
            [back]     NVARCHAR (50) NOT NULL,
            CONSTRAINT [PK_flashcards] PRIMARY KEY CLUSTERED ([id] ASC),
            CONSTRAINT [FK_flashcards_stacks] FOREIGN KEY ([stack_id]) REFERENCES [dbo].[stacks] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
        );
        ";
        command.ExecuteNonQuery();
        Logger.Info("Table 'flashcards' created.");
    }

    private static void CreateTableStudySessions(SqlConnection connection)
    {
        if (TableExists("study_sessions", connection))
        {
            return;
        }

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE [dbo].[study_sessions] (
            [id]            INT      NOT NULL,
            [stack_id]      INT      NOT NULL,
            [completed_at]  DATETIME NOT NULL,
            [score_percent] TINYINT  NOT NULL,
            CONSTRAINT [PK_study_sessions] PRIMARY KEY CLUSTERED ([id] ASC),
            CONSTRAINT [FK_study_sessions_stacks] FOREIGN KEY ([stack_id]) REFERENCES [dbo].[stacks] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
        );
        ";
        command.ExecuteNonQuery();
        Logger.Info("Table 'study_sessions' created.");
    }
}