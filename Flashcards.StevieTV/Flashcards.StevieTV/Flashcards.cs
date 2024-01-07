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
        InitialiseDatabase();
        Menu.MainMenu();

    }

    private static void InitialiseDatabase()
    {
        DatabaseManager.CreateDatabase();

        var stacksTable = @"Id int identity,
                            Name VARCHAR(50),
                            constraint Stacks_pk
                                primary key (Id)";

        DatabaseManager.CreateTable("Stacks", stacksTable);

        var cardsTable = @"Id int identity,
                            StackId int Not Null,
                            Front VARCHAR(Max) Not Null,
                            Back VARCHAR(Max) Not Null,
                            FOREIGN KEY (StackId) REFERENCES Stacks(Id) ON DELETE CASCADE";

        DatabaseManager.CreateTable("Cards", cardsTable);
    }
}