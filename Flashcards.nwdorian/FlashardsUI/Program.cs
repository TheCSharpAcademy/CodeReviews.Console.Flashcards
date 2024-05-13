using FlashardsUI;
using FlashcardsLibrary;

var database = new DatabaseManager();
database.InitDatabase();

var menu = new Menu();
await menu.MainMenu();