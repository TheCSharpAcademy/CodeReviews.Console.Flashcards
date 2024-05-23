using Flashcards.Database;
using Flashcards.Menus;

var databaseCreation = new DatabaseCreation();
var mainMenu = new MainMenuUI();

databaseCreation.CreateDatabase();
mainMenu.MainMenu();