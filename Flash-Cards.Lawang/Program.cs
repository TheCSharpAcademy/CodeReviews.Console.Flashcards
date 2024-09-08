using System.Configuration;
using Flash_Cards.Lawang;
using Flash_Cards.Lawang.Controller;
using Flash_Cards.Lawang.Views;

string connectionString = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;
var visualize = new Visualize();
var validation = new Validation();

var stackController  = new StackController(connectionString);
stackController.CreateStackTable();

//This value is added just for testing
stackController.SeedValueForTesting();

var flashCardController = new FlashCardController(connectionString);
flashCardController.CreateFlashCardTable();

//This value is added just for testing
flashCardController.SeedValueForTesting();

var studyController = new StudyController(connectionString);
studyController.CreateStudyTable();

var manager = new ApplicationManager(visualize, validation, stackController, flashCardController, studyController);
manager.Start();