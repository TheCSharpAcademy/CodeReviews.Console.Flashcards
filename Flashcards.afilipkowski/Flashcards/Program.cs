using Flashcards;

var db = new Database();
var userInterface = new UserInterface();

db.CreateDatabase();
db.CreateTables();
userInterface.MainMenu();