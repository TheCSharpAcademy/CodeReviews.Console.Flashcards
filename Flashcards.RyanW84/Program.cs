using Flashcards.RyanW84;

var DataAccess = new DataAccess();

DataAccess.CreateTables();

SeedData.SeedRecords();

UserInterface.MainMenu();
