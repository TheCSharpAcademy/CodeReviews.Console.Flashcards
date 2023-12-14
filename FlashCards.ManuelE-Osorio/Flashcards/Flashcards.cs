namespace Flashcards;

class Flashcards
{
    public static DataController ProgramController = new();
    public static string SelectedStack = "spanish";
    public static void Main()
    {
        DBController.DeleteStack("hola");                  
        ProgramController.MainMenuController();

    }
}


// CREATE TABLE stacks
// (
    // stackid INT IDENTITY(1,1) PRIMARY KEY,
    // stackname VARCHAR(50) NOT NULL,
// );
// GO

// INSERT INTO stacks
// (stackname)
// VALUES
// ('spanish'), ('english'), ('russian')
// GO


// USE master;
// GO
// IF NOT EXISTS (
    // SELECT name
        // FROM sys.databases
        // WHERE name = N'FlashCardsProgram'
// )
// CREATE DATABASE FlashCardsProgram
// GO

