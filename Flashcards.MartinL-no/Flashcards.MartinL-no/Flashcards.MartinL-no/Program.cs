using Flashcards.MartinL_no.DAL;

var connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
var stackRepo = new FlashcardRepository(connectionString);
Console.WriteLine("Hello, World!");