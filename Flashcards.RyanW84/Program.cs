using Flashcards.RyanW84;

var dataAccess = new DataAccess();

dataAccess.ConfirmConnection();

dataAccess.CreateTables();

//SeedData.SeedRecords();

UserInterface.MainMenu();
