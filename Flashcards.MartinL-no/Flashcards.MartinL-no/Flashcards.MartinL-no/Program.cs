using Flashcards.MartinL_no.DAL;
using Flashcards.MartinL_no.Controllers;
using Flashcards.MartinL_no.UserInterface;

var connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

var stackRepo = new FlashcardStackRepository(connectionString);
var stackManagerController = new StackManagerController(stackRepo);
var stackManagerApp = new StackManagerApplication(stackManagerController);

var sessionRepo = new StudySessionRepository(connectionString);
var studySessionController = new StudySessionController(sessionRepo);
var sessionApp = new StudySessionApplication(studySessionController, stackManagerController);

var ui = new UserInput(stackManagerApp, sessionApp);

ui.Menu();
