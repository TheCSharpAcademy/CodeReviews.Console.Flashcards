using QC = Microsoft.Data.SqlClient;
using DT = System.Data;
using Microsoft.Data.SqlClient;

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
                            [Id] INT IDENTITY (1, 1) NOT NULL,
                            [Name] NVARCHAR (100) NOT NULL,
                            PRIMARY KEY CLUSTERED ([Id] ASC),
                            CONSTRAINT [AK_Name] UNIQUE NONCLUSTERED ([Name] ASC)
                        );
                    END;
                    """;

                command.ExecuteNonQuery();

                command.CommandText = """
                    IF OBJECT_ID(N'[dbo].[Flashcard]', N'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[Flashcard] (
                            [Id] INT IDENTITY (1, 1) NOT NULL,
                            [Front] NVARCHAR (MAX) NOT NULL,
                            [Back] NVARCHAR (MAX) NOT NULL,
                            [StackId] INT NOT NULL,
                            PRIMARY KEY CLUSTERED ([Id] ASC),
                            CONSTRAINT [FK_Flashcard_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([Id]) ON DELETE CASCADE
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
                    SELECT s.[Id] AS [StackId], s.[Name], f.[Id] AS [FlashcardId], f.[Front], f.[Back]
                    FROM
                        [dbo].[Stack] s
                    LEFT JOIN
                        [dbo].[Flashcard] f ON s.[Id] = f.[StackId];
                    """;

                using (var reader = command.ExecuteReader())
                {
                    int stackId = 0;
                    string name = null;
                    var stacks = new List<FlashcardStack>();
                    var flashcards = new List<Flashcard>();

                    while (reader.Read())
                    {
                        if (stackId != 0 && (int) reader["StackId"] != stackId)
                        {
                            stacks.Add(new FlashcardStack()
                            {
                                Id = stackId,
                                Name = name,
                                Flashcards = flashcards
                            });

                            flashcards = new List<Flashcard>();
                        }

                        stackId = (int) reader["StackId"];
                        name = (string) reader["Name"];

                        var hasFlashcard = !reader.IsDBNull(2);
                        if (hasFlashcard)
                        {
                            flashcards.Add(new Flashcard()
                            {
                                Id = (int) reader["FlashcardId"],
                                Front = (string) reader["Front"],
                                Back = (string) reader["Back"],
                                StackId = stackId
                            });
                        }
                    }

                    if (reader.HasRows == true)
                    {
                        stacks.Add(new FlashcardStack()
                        {
                            Id = stackId,
                            Name = name,
                            Flashcards = flashcards
                        });
                    }

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
                    SELECT s.[Id] AS [StackId], f.[Id] AS [FlashcardId], f.[Front], f.[Back]
                    FROM
                        [dbo].[Stack] s
                    LEFT JOIN
                        [dbo].[Flashcard] f ON s.[Id] = f.[StackId]
                    WHERE
                        s.[Name] = @name
                    """;

                var parameter = new QC.SqlParameter("@name", DT.SqlDbType.NVarChar);
                parameter.Value = name;
                command.Parameters.Add(parameter);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;

                    int stackId = 0; 
                    var flashcards = new List<Flashcard>();

                    while (reader.Read())
                    {
                        stackId = (int) reader["StackId"];

                        var hasFlashcard = !reader.IsDBNull(1);
                        if (hasFlashcard)
                        {
                            flashcards.Add(new Flashcard()
                            {
                                Id = (int) reader["FlashcardId"],
                                Front = (string) reader["Front"],
                                Back = (string) reader["Back"],
                                StackId = stackId
                            });
                        }
                    }

                    return new FlashcardStack()
                    {
                        Id = stackId,
                        Name = name,
                        Flashcards = flashcards
                    };
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
                    return command.ExecuteNonQuery() > 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }

    public bool InsertFlashcard(Flashcard flashCard)
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
                        ([Front], [Back], [StackId])
                    VALUES
                        (@front, @back, @stackId)
                    """;

                var parameter = new QC.SqlParameter("@front", DT.SqlDbType.NVarChar);
                parameter.Value = flashCard.Front;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@back", DT.SqlDbType.NVarChar);
                parameter.Value = flashCard.Back;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@stackId", DT.SqlDbType.Int);
                parameter.Value = flashCard.StackId;
                command.Parameters.Add(parameter);

                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }

    public bool UpdateFlashcard(Flashcard flashCard)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    UPDATE [dbo].[Flashcard]
                    SET Front = @front,
                        Back = @back,
                        StackId = @stackId
                    WHERE
                        Id = @id;
                    """;

                var parameter = new QC.SqlParameter("@id", DT.SqlDbType.Int);
                parameter.Value = flashCard.Id;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@front", DT.SqlDbType.NVarChar);
                parameter.Value = flashCard.Front;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@back", DT.SqlDbType.NVarChar);
                parameter.Value = flashCard.Back;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@stackId", DT.SqlDbType.Int);
                parameter.Value = flashCard.StackId;
                command.Parameters.Add(parameter);

                try
                {
                    return command.ExecuteNonQuery() > 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }

    public bool DeleteStack(int id)
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
                    WHERE Id = @id
                    """;

                var parameter = new QC.SqlParameter("@id", DT.SqlDbType.Int);
                parameter.Value = id;
                command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool DeleteFlashCard(int id)
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
                    WHERE Id = @id;
                    """;

                var parameter = new QC.SqlParameter("@id", DT.SqlDbType.Int);
                parameter.Value = id;
                command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}