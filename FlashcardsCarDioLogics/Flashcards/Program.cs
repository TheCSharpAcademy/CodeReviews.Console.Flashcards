using Flashcards;

UI userInterface = new UI();
DatabaseLogic databaseLogic = new DatabaseLogic();

databaseLogic.CreateDatabase();

databaseLogic.CreateTables();

userInterface.MainMenu();
