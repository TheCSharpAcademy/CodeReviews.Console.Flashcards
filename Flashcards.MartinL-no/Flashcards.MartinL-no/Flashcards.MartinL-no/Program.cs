using Flashcards.MartinL_no.DAL;
using Flashcards.MartinL_no.Controllers;
using Flashcards.MartinL_no.UserInterface;


var connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
var stackRepo = new FlashcardStackRepository(connectionString);
var controller = new FlashcardController(stackRepo);
var ui = new UserInput(controller);

ui.Execute();