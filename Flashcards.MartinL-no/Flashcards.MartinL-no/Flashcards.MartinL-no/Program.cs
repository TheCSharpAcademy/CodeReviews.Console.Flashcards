using Flashcards.MartinL_no.DAL;
using Flashcards.MartinL_no.Controllers;
using Flashcards.MartinL_no.UserInterface;

var connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

var stackRepo = new FlashcardStackRepository(connectionString);
var stackManagerController = new StackManagerController(stackRepo);
var stackManager = new StackManager(stackManagerController);

var ui = new UserInput(stackManager);

ui.Menu();