namespace Buutyful.Coding_Tracker;

public static class Constants
{
    //Create Deck, FlashCard and StudySessions tables sql command
    public static string CreateTablesSql => @"
          CREATE TABLE Decks (
              Id INT IDENTITY(1,1) PRIMARY KEY,
              Name NVARCHAR(50) NOT NULL,
              Category NVARCHAR(50) NOT NULL);

          CREATE TABLE FlashCards (
              Id INT IDENTITY(1,1) PRIMARY KEY,
              DeckId INT,
              FrontQuestion NVARCHAR(255) NOT NULL,
              BackAnswers NVARCHAR(MAX) NOT NULL,
              CorrectAnswer INT,
              FOREIGN KEY (DeckId) REFERENCES Decks(Id));

          CREATE TABLE StudySessions (
              Id INT IDENTITY(1,1) PRIMARY KEY,
              DeckId INT,
              CreatedAt DATETIME,
              Score INT,
              FOREIGN KEY (DeckId) REFERENCES Decks(Id));";

    //Check if Deck And FlashCard tables already exist sql command
    public static string CheckTableExists => @"
        SELECT COUNT(*) 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME IN ('Decks', 'FlashCards', 'StudySessions');";
}
