using QC = Microsoft.Data.SqlClient;
using DT = System.Data;

using Flashcards.MartinL_no.DAL.Models;
using System.Reflection.Metadata;

namespace Flashcards.MartinL_no.DAL;

internal class FlashcardStackRepository
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
                            [id]   INT            IDENTITY (1, 1) NOT NULL,
                            [Name] NVARCHAR (100) NOT NULL,
                            PRIMARY KEY CLUSTERED ([id] ASC),
                            CONSTRAINT [AK_Name] UNIQUE NONCLUSTERED ([Name] ASC)
                        );
                    END;
                    """;

                command.ExecuteNonQuery();

                command.CommandText = """
                    IF OBJECT_ID(N'[dbo].[Flashcard]', N'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[Flashcard] (
                            [Text]    TEXT NOT NULL,
                            [StackId] INT  NOT NULL,
                            CONSTRAINT [FK_Flashcard_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([id]) ON DELETE CASCADE
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
                    SELECT s.[id], s.[Name], f.[Text]
                    FROM
                        [dbo].[Stack] s
                    INNER JOIN
                        [dbo].[Flashcard] f ON s.[id] = f.[StackId];
                    """;

                using (var reader = command.ExecuteReader())
                {
                    var stacks = new List<FlashcardStack>();
                    var id = 0;
                    var name = "";
                    var flashcards = new Stack<string>();

                    while (reader.Read())
                    {
                        if (id != 0 && reader.GetInt32(0) != id)
                        {
                            stacks.Add(new FlashcardStack(id, name, flashcards));
                            flashcards = new Stack<string>();
                        }

                        id = reader.GetInt32(0);
                        name = reader.GetString(1);
                        flashcards.Push(reader.GetString(2));
                    }

                    if (reader.HasRows == true) stacks.Add(new FlashcardStack(id, name, flashcards));

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
                    SELECT s.[id], s.[Name], f.[Text]
                    FROM
                        [dbo].[Stack] s
                    INNER JOIN
                        [dbo].[Flashcard] f ON s.[id] = f.[StackId]
                    WHERE
                        s.[Name] = @name
                    """
                ;

                var parameter = new QC.SqlParameter("@name", DT.SqlDbType.NVarChar);
                parameter.Value = name;
                command.Parameters.Add(parameter);

                using (var reader = command.ExecuteReader())
                {
                    var id = 0;
                    var flashcards = new Stack<string>();

                    while (reader.Read())
                    {
                        if (!reader.HasRows) return null;

                        id = reader.GetInt32(0);
                        flashcards.Push(reader.GetString(2));
                    }

                    return new FlashcardStack(id, name, flashcards);
                }
            }
        }
    }
}