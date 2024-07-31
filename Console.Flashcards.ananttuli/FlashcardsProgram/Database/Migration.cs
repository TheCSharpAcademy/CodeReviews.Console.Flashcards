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
                WHERE name = '{StackDao.TableName}' AND schema_id = SCHEMA_ID('dbo')
            )
                BEGIN
                   CREATE TABLE {StackDao.TableName} (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(255) UNIQUE NOT NULL
                    );
                END

            IF NOT EXISTS (
                SELECT * FROM sys.tables
                WHERE name = '{FlashcardDao.TableName}' AND schema_id = SCHEMA_ID('dbo')
            )
                BEGIN
                   CREATE TABLE {FlashcardDao.TableName} (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Front NVARCHAR(1024) NOT NULL,
                        Back NVARCHAR(1024) NOT NULL,
                        StackId INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES {StackDao.TableName}(Id) ON DELETE CASCADE
                    );
                END

            IF NOT EXISTS (
                SELECT * FROM sys.tables
                WHERE name = '{StudySessionDao.TableName}' AND schema_id = SCHEMA_ID('dbo')
            )
                BEGIN
                   CREATE TABLE {StudySessionDao.TableName} (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        NumAttempted INT NOT NULL,
                        NumCorrect INT NOT NULL,
                        DateTime  DATETIME2 NULL,
                        StackId INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES {StackDao.TableName}(Id) ON DELETE CASCADE
                    );
                END
        ";

        connection.Execute(sql);
    }


    public static void DropTables(SqlConnection connection)
    {
        connection.Execute($@"
            DROP TABLE {FlashcardDao.TableName};
            DROP TABLE {StudySessionDao.TableName};
            DROP TABLE {StackDao.TableName};
        ");
    }

}