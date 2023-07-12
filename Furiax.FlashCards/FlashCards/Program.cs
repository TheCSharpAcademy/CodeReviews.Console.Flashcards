using System.Configuration;
using FlashCards;

var connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;
Console.Title = "Flashcards";
DataAccess.SetupDbAndTables(connectionString);
UserInput.GetMenuInput(connectionString);
