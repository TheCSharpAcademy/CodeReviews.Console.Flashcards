using Flashcards.AnaClos.Controllers;

string choice;
string title = "Select a function";
string color = "green";

var mainOptions = new List<string> { "Add a Stack", "Delete a Stack", "Add a Flash Card",
        "Delete a Flash Card", "Study Session", "View Study Sessions by Stack", "Exit" };

var consoleController = new ConsoleController();
var dataBaseController = new DataBaseController();
var stackController = new StackController(consoleController,dataBaseController);
var flashCardController = new FlashCardController(consoleController, dataBaseController,stackController);
var studySessionController = new StudySessionController(consoleController,dataBaseController,stackController,flashCardController);

dataBaseController.CreateDatabase();
dataBaseController.CreateTableStacks();
dataBaseController.CreateTableFlashCards();
dataBaseController.CreateTableStudySessions();
flashCardController.InitFlashCardsDto();
do
{
    choice = consoleController.Menu(title,color,mainOptions);
    
    switch (choice){
        case "Add a Stack":
            stackController.AddStack();
            break;
        case "Delete a Stack":
            stackController.DeleteStack();
            break;
        case "Add a Flash Card":
            flashCardController.AddFlashCard();
            break;
        case "Delete a Flash Card":
            flashCardController.DeleteFlashCard();
            break;
        case "Study Session":
            studySessionController.AddStudySession();
            break;
        case "View Study Sessions by Stack":
            studySessionController.ViewStudySession();
            break;
    }
} while (choice != "Exit");