using Flashcards;

DatabaseManager dbManager = new();
MainMenu mainMenu = new();

dbManager.CreateDb("FlashcardsProject");
dbManager.CreateTables();

mainMenu.PrintMenu();



