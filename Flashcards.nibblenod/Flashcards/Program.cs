using Flashcards;

DatabaseManager dbManager = new();
MainMenu mainMenu = new();

dbManager.createDB("FlashcardsProject");
dbManager.createTables();

mainMenu.PrintMenu();



