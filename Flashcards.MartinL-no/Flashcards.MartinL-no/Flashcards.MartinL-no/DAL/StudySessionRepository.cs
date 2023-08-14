using QC = Microsoft.Data.SqlClient;
using DT = System.Data;
using Microsoft.Data.SqlClient;
using Flashcards.MartinL_no.Models;

namespace Flashcards.MartinL_no.DAL;

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
                            CONSTRAINT [FK_StudySession_Stack] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stack] ([Id]) ON DELETE CASCADE
                        );
                    END;
                    """;

                command.ExecuteNonQuery();
            }
        }
    }

    public List<StudySession> GetSessions()
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    SELECT ss.[Id], ss.[Date], ss.[Score], ss.[StackId]
                    FROM
                        [dbo].[StudySession] ss
                    """;

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    var studySessions = new List<StudySession>();

                    while (reader.Read())
                    {
                        var studySession = new StudySession()
                        {
                            Id = (int)reader["Id"],
                            Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                            Score = (int)reader["Score"],
                            StackId = (int)reader["StackId"],
                        };

                        studySessions.Add(studySession);
                    }

                    return studySessions;
                }
            }
        }
    }

    public List<StudySession> GetSessionByName(string name)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    SELECT ss.[Id], ss.[Date], ss.[Score], ss.[StackId]
                    FROM
                        [dbo].[StudySession] ss
                    INNER JOIN
                        [dbo].[Stack] st ON st.Id = ss.[StackId]
                    WHERE
                        st.[Name] = @name
                    """;

                var parameter = new QC.SqlParameter("@name", DT.SqlDbType.NVarChar);
                parameter.Value = name;
                command.Parameters.Add(parameter);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;

                    var studySessions = new List<StudySession>();

                    while (reader.Read())
                    {
                        var studySession = new StudySession()
                        {
                            Id = (int)reader["Id"],
                            Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                            Score = (int)reader["Score"],
                            StackId = (int)reader["StackId"],
                        };

                        studySessions.Add(studySession);
                    }

                    return studySessions;
                }
            }
        }
    }

    public bool InsertStudySession(StudySession studySession)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    INSERT INTO [dbo].[StudySession]
                        ([Date], [Score], [StackId])
                    VALUES
                        (@date, @score, @stackId)
                    """;

                var parameter = new QC.SqlParameter("@date", DT.SqlDbType.Date);
                parameter.Value = studySession.Date;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@score", DT.SqlDbType.Int);
                parameter.Value = studySession.Score;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@stackId", DT.SqlDbType.Int);
                parameter.Value = studySession.StackId;
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

    public bool UpdateStudySession(StudySession studySession)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    UPDATE [dbo].[StudySession]
                    SET [Date] = @date,
                        [Score] = @score,
                        [StackId] = @stackId
                    WHERE
                        [Id] = @id;
                    """;

                var parameter = new QC.SqlParameter("@id", DT.SqlDbType.Int);
                parameter.Value = studySession.Id;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@date", DT.SqlDbType.Date);
                parameter.Value = studySession.Date;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@score", DT.SqlDbType.Int);
                parameter.Value = studySession.Score;
                command.Parameters.Add(parameter);

                parameter = new QC.SqlParameter("@stackId", DT.SqlDbType.Int);
                parameter.Value = studySession.StackId;
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
    public bool DeleteStudySession(int id)
    {
        using (var connection = new QC.SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new QC.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = DT.CommandType.Text;
                command.CommandText = """
                    DELETE FROM [dbo].[StudySession]
                    WHERE Id = @id
                    """;

                var parameter = new QC.SqlParameter("@id", DT.SqlDbType.Int);
                parameter.Value = id;
                command.Parameters.Add(parameter);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }

}