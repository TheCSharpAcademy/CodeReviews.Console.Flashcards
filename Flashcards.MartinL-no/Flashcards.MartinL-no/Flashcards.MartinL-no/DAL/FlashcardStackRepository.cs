using QC = Microsoft.Data.SqlClient;
using DT = System.Data;

using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.DAL;

internal class FlashcardStackRepository : IFlashcardStackRepository
{
    private readonly string _connectionString;

    public FlashcardStackRepository(string connectionString)
    {
        _connectionString = connectionString;
        CreateTables();
    }

    private void CreateTables()
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;

                command.CommandText = """
                    IF OBJECT_ID(N'[dbo].[Stack]', N'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[Stack] (
                            [Name] NVARCHAR (100) NOT NULL,
                            PRIMARY KEY CLUSTERED ([Name] ASC),
                            CONSTRAINT [AK_Name] UNIQUE NONCLUSTERED ([Name] ASC)
                        );
                    END;
                    """;

                command.ExecuteNonQuery();

                command.CommandText = """
                    IF OBJECT_ID(N'[dbo].[Flashcard]', N'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[Flashcard] (
                            [Text]    NVARCHAR (MAX) NOT NULL,
                            [StackName] NVARCHAR (100)  NOT NULL,
                            CONSTRAINT [FK_Flashcard_Stack] FOREIGN KEY ([StackName]) REFERENCES [dbo].[Stack] ([Name]) ON DELETE CASCADE
                        );
                    END;
                    """;

                command.ExecuteNonQuery();
            }
        }
    }

    public List<FlashcardStack> GetStacks()
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    SELECT s.[Name], f.[Text]
                    FROM
                        [dbo].[Stack] s
                    INNER JOIN
                        [dbo].[Flashcard] f ON s.[Name] = f.[StackName];
                    """;

                using (var reader = command.ExecuteReader())
                {
                    var stacks = new List<FlashcardStack>();
                    string? name = null;
                    var flashcards = new Stack<string>();

                    while (reader.Read())
                    {
                        if (name != null && reader.GetString(0) != name)
                        {
                            stacks.Add(new FlashcardStack(name, flashcards));
                            flashcards = new Stack<string>();
                        }

                        name = reader.GetString(0);
                        flashcards.Push(reader.GetString(1));
                    }

                    if (reader.HasRows == true) stacks.Add(new FlashcardStack(name, flashcards));

                    return stacks;
                }
            }
        }
    }

    public FlashcardStack GetStackByName(string name)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    SELECT s.[Name], f.[Text]
                    FROM
                        [dbo].[Stack] s
                    INNER JOIN
                        [dbo].[Flashcard] f ON s.[Name] = f.[StackName]
                    WHERE
                        s.[Name] = @name
                    """
                ;

                var parameter = new QC.SqlParameter("@name", DT.SqlDbType.NVarChar);
                parameter.Value = name;
                command.Parameters.Add(parameter);

                using (var reader = command.ExecuteReader())
                {
                    var flashcards = new Stack<string>();

                    while (reader.Read())
                    {
                        if (!reader.HasRows) return null;

                        flashcards.Push(reader.GetString(1));
                    }

                    return new FlashcardStack(name, flashcards);
                }
            }
        }
    }

    public bool InsertStack(FlashcardStack stack)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    INSERT INTO [dbo].[Stack]
                        ([Name])
                    VALUES
                        (@name)
                    """;

                var parameter = new QC.SqlParameter("@name", DT.SqlDbType.NVarChar);
                parameter.Value = stack.Name;
                command.Parameters.Add(parameter);

                try
                {
                    var rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0) return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        foreach (var flashcard in stack.Flashcards)
        {
            var insertedSuccessfully = InsertFlashcard(flashcard, stack.Name);
            if (!insertedSuccessfully) return false;
        }

        return true;
    }

    public bool InsertFlashcard(string text, string stackName)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    INSERT INTO [dbo].[Flashcard]
                        ([Text], [StackName])
                    VALUES
                        (@text, @stackName)
                    """;

                var parameter = new QC.SqlParameter("@text", DT.SqlDbType.NVarChar);
                parameter.Value = text;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@stackName", DT.SqlDbType.NVarChar);
                parameter.Value = stackName;
                command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool DeleteStack(FlashcardStack stack)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    DELETE FROM [dbo].[Stack]
                    WHERE Name = @name
                    """;

                var parameter = new QC.SqlParameter("@name", DT.SqlDbType.NVarChar);
                parameter.Value = stack.Name;
                command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool DeleteFlashCard(string text, string stackName)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    DELETE FROM [dbo].[Flashcard]
                    WHERE Text = @text
                    """;

                var parameter = new QC.SqlParameter("@text", DT.SqlDbType.NVarChar);
                parameter.Value = text;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@stackName", DT.SqlDbType.NVarChar);
                parameter.Value = stackName;
                command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}