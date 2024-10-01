using Flashcards.Models;
using Microsoft.Data.SqlClient;

namespace Flashcard_Application.DataServices;

internal class CreateDatabaseAndTable
{
    public static void DbAndTableCreation()
    {
        var connectionString = DatabaseConfig.dbFilePath;
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @"
            IF OBJECT_ID('Stacks') IS NULL
            BEGIN
            CREATE TABLE Stacks (
                StackId int PRIMARY KEY IDENTITY(1,1), 
                StackName nvarchar(max),     
                StackDescription nvarchar(max),
                );
            END
            ";
        command.ExecuteNonQuery();

        command.CommandText =
            @"
            IF OBJECT_ID('Flashcards') IS NULL
            BEGIN
            CREATE TABLE Flashcards (
                CardId int PRIMARY KEY IDENTITY(1,1), 
                StackId int,
                Question nvarchar(max),     
                Answer nvarchar(max),
                FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE ON UPDATE CASCADE,
                );
            END
            ";
        command.ExecuteNonQuery();

        command.CommandText =
            @"
            IF OBJECT_ID('StudySession') IS NULL
            BEGIN
            CREATE TABLE StudySession (
                StudySessionId int PRIMARY KEY IDENTITY, 
                StackId int,
                StudySessionStartTime datetime,     
                StudySessionEndTime datetime,
                StudySessionScore int,
                FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE ON UPDATE CASCADE,
                );
            END
            ";
        command.ExecuteNonQuery();

        DatabaseSeeding seeding = new DatabaseSeeding();
        CardStack[] cardStacks = new CardStack[] { seeding.cardStack1, seeding.cardStack2, };
        Flashcard[] flashcards = new Flashcard[] { seeding.intCard, seeding.stringCard, seeding.boolCard, seeding.charCard, seeding.publicCard, seeding.privateCard, seeding.protectedCard, seeding.internalCard };

        foreach (var stack in cardStacks)
        {
            using (var seedconnection = new SqlConnection(DatabaseConfig.dbFilePath))
            {
                using var sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "IF NOT EXISTS(Select StackName from Stacks where StackName = @StackName) BEGIN INSERT INTO Stacks(StackName, StackDescription) VALUES(@StackName, @StackDescription) END";
                sqlCommand.Parameters.AddWithValue("@StackName", stack.StackName);
                sqlCommand.Parameters.AddWithValue("@StackDescription", stack.StackDescription);
                sqlCommand.ExecuteNonQuery();
            }
        }

        foreach (var flashcard in flashcards)
        {
            using (var cardconnection = new SqlConnection(DatabaseConfig.dbFilePath))
            {
                using var sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "IF NOT EXISTS(Select Question from Flashcards where Question=@Question) BEGIN INSERT INTO Flashcards(Question, Answer, StackId) VALUES(@Question, @Answer, @StackId) END";
                sqlCommand.Parameters.AddWithValue("@Question", flashcard.Question);
                sqlCommand.Parameters.AddWithValue("@Answer", flashcard.Answer);
                sqlCommand.Parameters.AddWithValue("@StackId", flashcard.StackId);

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
