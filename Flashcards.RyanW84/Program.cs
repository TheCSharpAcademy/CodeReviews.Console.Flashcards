using Flashcards.RyanW84;

var DataAccess = new DataAccess();

DataAccess.ConfirmConnection();

DataAccess.CreateTables();

UserInterface.MainMenu();
