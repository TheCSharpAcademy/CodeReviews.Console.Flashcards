using QC = Microsoft.Data.SqlClient;
using DT = System.Data;
using Microsoft.Data.SqlClient;

internal class StudySessionRepository
{
    private readonly string _connectionString;

    public StudySessionRepository(string connectionString)
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
                    IF OBJECT_ID(N'[dbo].[StudySession]', N'U') IS NULL
                    BEGIN
                        CREATE TABLE [dbo].[StudySession] (
                            [Id]      INT  IDENTITY (1, 1) NOT NULL,
                            [Date]    DATE NOT NULL,
                            [Score]   INT  NOT NULL,
                            [StackId] INT  NOT NULL,
                            CONSTRAINT [PK_StudySession] PRIMARY KEY CLUSTERED ([Id] ASC),
                            CONSTRAINT [FK_StudySession_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([Id])
                        );
                    END;
                    """;

                command.ExecuteNonQuery();
            }
        }
    }
}