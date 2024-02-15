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
              BackAnswer NVARCHAR(255) NOT NULL,             
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
    //Seed for Deck table
    public static string SeedDeckSql => @"
            INSERT INTO Decks (Name, Category) VALUES
            ('Math Basics', 'Education'),
            ('Science Quiz', 'Education'),
            ('General Knowledge', 'Trivia');";
    //Seed for FlashCards table
    public static string SeedFlashCards => @"
            INSERT INTO FlashCards (DeckId, FrontQuestion, BackAnswer) VALUES
            (1, 'What is 2 + 2?', '4'),
            (1, 'What is 5 x 5?', '25'),
            (2, 'Who discovered gravity?', 'Isaac Newton'),
            (2, 'What is the chemical symbol for water?', 'H2O'),
            (3, 'What is the capital of France?', 'Paris');";
    public static string FlashCardsTable => "FlashCards";
    public static string DecksTable => "Decks";
    public static string StudySessionsTable => "StudySessions";
}
