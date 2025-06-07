using Flashcards.Data;
using Flashcards.Views;

var Menu = new Menu();
var DataConnection = new DataConnection();

DataConnection.CreateDatabase();
SeedData.SeedFlashcards();
Menu.MainMenu();