using cacheMe512.Flashcards;
using cacheMe512.Flashcards.UI;

Console.WriteLine("Initializing database...");
Database.InitializeDatabase();

Console.WriteLine("Database initialized successfully.");
Console.WriteLine("Starting App...");

var mainMenu = new MainMenu();
mainMenu.Show();
