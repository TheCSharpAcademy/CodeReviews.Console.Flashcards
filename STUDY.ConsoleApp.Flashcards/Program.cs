using Spectre.Console;
using STUDY.ConsoleApp.Flashcards;
using STUDY.ConsoleApp.Flashcards.UI;

DatabaseHelper databaseHelper = new();
databaseHelper.CreateTables();

AnsiConsole.Markup("Welcome to [green]Flashcard[/] application.\n");
Menus.MainMenu();