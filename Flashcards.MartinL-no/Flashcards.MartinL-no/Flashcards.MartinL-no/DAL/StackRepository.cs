using QC = Microsoft.Data.SqlClient;
using DT = System.Data;

namespace Flashcards.MartinL_no.DAL;

internal class FlashcardRepository
{
    private readonly string _connectionString;

    public FlashcardRepository(string connectionString)
    {
        _connectionString = connectionString;
        CreateTable();
    }
    private void CreateTable()
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    IF OBJECT_ID(N'[dbo].[Flashcard]', N'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[Flashcard] (
                            [id]      INT  IDENTITY (1, 1) NOT NULL,
                            [Text]    TEXT NOT NULL,
                            [StackId] INT  NOT NULL,
                            PRIMARY KEY CLUSTERED ([id] ASC),
                            CONSTRAINT [FK_Flashcard_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([id]) ON DELETE CASCADE
                        );
                    END;
                    """
                ;

                command.ExecuteNonQuery();
            }
        }
    }
}