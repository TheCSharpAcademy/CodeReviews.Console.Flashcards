using System.Configuration;
using Flashcards.StevieTV.Database;
using Flashcards.StevieTV.UI;

namespace Flashcards.StevieTV;

internal static class Flashcards
{
    private static readonly string DatabaseName = ConfigurationManager.AppSettings.Get("Database");
    public static readonly DatabaseManager DatabaseManager = new DatabaseManager(DatabaseName);

    private static void Main()
    {
        Console.Title = "Flash Cards by StevieTV";
        InitialiseDatabase();
        Menu.MainMenu();

    }

    private static void InitialiseDatabase()
    {
        DatabaseManager.CreateDatabase();

        const string stacksTable = @"Id int identity,
                            Name VARCHAR(50),
                            constraint Stacks_pk
                                primary key (Id)";

        DatabaseManager.CreateTable("Stacks", stacksTable);

        const string cardsTable = @"Id int identity,
                            StackId int Not Null,
                            Front VARCHAR(Max) Not Null,
                            Back VARCHAR(Max) Not Null,
                            FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE";

        DatabaseManager.CreateTable("Cards", cardsTable);

        const string studySessionsTable = @"Id int identity,
                            Date DateTime Not Null,
                            StackId int Not Null,
                            Score int Not Null,
                            QuantityTested int Not Null,
                            FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE";
        
        DatabaseManager.CreateTable("StudySessions", studySessionsTable);
    }
}