using Dapper;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;
using Microsoft.Data.SqlClient;

namespace FlashcardsProgram.Database;

public class Migration
{
    public static void CreateTablesIfNotExists(SqlConnection connection)
    {
        string sql = $@"
            IF NOT EXISTS (
                SELECT * FROM sys.tables
                WHERE name = '{StackDAO.TableName}' AND schema_id = SCHEMA_ID('dbo')
            )
                BEGIN
                   CREATE TABLE {StackDAO.TableName} (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(255) UNIQUE NOT NULL
                    );
                END

            IF NOT EXISTS (
                SELECT * FROM sys.tables
                WHERE name = '{FlashcardDAO.TableName}' AND schema_id = SCHEMA_ID('dbo')
            )
                BEGIN
                   CREATE TABLE {FlashcardDAO.TableName} (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Front NVARCHAR(1024) NOT NULL,
                        Back NVARCHAR(1024) NOT NULL,
                        StackId INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES {StackDAO.TableName}(Id) ON DELETE CASCADE
                    );
                END

            IF NOT EXISTS (
                SELECT * FROM sys.tables
                WHERE name = '{StudySessionDAO.TableName}' AND schema_id = SCHEMA_ID('dbo')
            )
                BEGIN
                   CREATE TABLE {StudySessionDAO.TableName} (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Score DECIMAL NOT NULL,
                        DateTime  DATETIME2 NULL,
                        StackId INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES {StackDAO.TableName}(Id) ON DELETE CASCADE
                    );
                END
        ";

        connection.Execute(sql);
    }
}