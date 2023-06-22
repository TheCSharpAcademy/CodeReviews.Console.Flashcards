using Microsoft.Extensions.Configuration;

namespace Ohshie.FlashCards.DataAccess;

public class DbBase
{
    protected string? ConnectionString { get; }

    protected DbBase()
    {
        ConnectionString = GetConnectionStringFromSettings();
    }
    
    private string? GetConnectionStringFromSettings()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        return configuration.GetConnectionString("MSSql");
    }

    protected readonly Dictionary<string, string> SqlCommands = new()
    {
        ["createDeckTable"] = "IF OBJECT_ID('dbo.Decks', 'U') IS NULL " +
                               "CREATE TABLE Decks " +
                               "(" +
                                   "Id INT PRIMARY KEY NOT NULL IDENTITY(1,1), " +
                                   "Name NVARCHAR(50) NOT NULL, " +
                                   "Description NVARCHAR(200)" +
                               ");",
        ["createFlashcardsTable"] = "IF OBJECT_ID('dbo.FlashCards', 'U') IS NULL " +
                                    "CREATE TABLE FlashCards " +
                                    "(" +
                                        "Id INT PRIMARY KEY NOT NULL IDENTITY(1,1), " +
                                        "Name NVARCHAR(100) NOT NULL, " +
                                        "Content NVARCHAR(Max)," +
                                        "DeckId INT NOT NULL," +
                                        "FOREIGN KEY (DeckId) REFERENCES Decks(Id) " +
                                    "ON DELETE CASCADE" +
                                    ");",
        ["fetchAllDecks"] = "SELECT * FROM Decks " +
                            "INNER JOIN FlashCards FC ON Decks.Id = FC.DeckId; ",
        
        ["fetchOneDeckById"] = "SELECT * FROM Decks " +
                                "INNER JOIN FlashCards ON (Decks.Id = FlashCards.DeckId) " +
                                "WHERE Decks.Id = "
    };
}