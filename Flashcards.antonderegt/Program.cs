using Flashcards;

DataAccess dataAccess = new();
dataAccess.CreateDatabase();
dataAccess.InitializeTables();

UserInterface.ShowMainMenu();