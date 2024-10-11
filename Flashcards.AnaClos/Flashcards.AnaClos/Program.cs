using Flashcards.AnaClos;
using Flashcards.AnaClos.Controllers;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

string choice;
int choiceIndex;
string server = ConfigurationManager.AppSettings.Get("Server");
string dataBaseName = ConfigurationManager.AppSettings.Get("DataBase");

string connectionString = $@"Server={server};Integrated Security=true;TrustServerCertificate=true";

string createDataBase = $@"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dataBaseName}')
BEGIN
CREATE DATABASE {dataBaseName};
END";

string createTableStacks = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stacks')
BEGIN
CREATE TABLE Stacks (
Id INT PRIMARY KEY IDENTITY,
Name NVARCHAR(50) NOT NULL,
CONSTRAINT Stacks_Name UNIQUE(Name)
);
END";

string deleteTableStacks = $"DROP TABLE Stacks;";

string createTableFlashCards = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FlashCards')
BEGIN
CREATE TABLE FlashCards (
Id INT PRIMARY KEY IDENTITY,
Front NVARCHAR(50) NOT NULL,
Back NVARCHAR(50) NOT NULL,
StackId INT NOT NULL,
CONSTRAINT Front_Name UNIQUE(Front),
CONSTRAINT fkStack FOREIGN KEY (StackId)
REFERENCES Stacks(Id)
ON DELETE CASCADE
);
END";

var mainOptions = new List<string> { "Add a Stack", "Delete a Stack", "Add a Flash Card",
        "Delete a Flash Card", "Study Session", "View Study Sessions by Stack",
        "Average Score Yearly Report", "Exit" };

var uiController = new ConsoleController();
var dataBaseController = new DataBaseController(connectionString);
var stackController = new StackController(uiController,dataBaseController);
var flashCardController = new FlashCardController(uiController, dataBaseController,stackController);

dataBaseController.Execute(createDataBase);
//dataBaseController.Execute(deleteTableStacks);
dataBaseController.Execute(createTableStacks);
dataBaseController.Execute(createTableFlashCards);
do
{
    choice = uiController.MainMenu(mainOptions);
    switch(choice){
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
            break;
        case "View Study Sessions by Stack":
            break;
        case "Average Score Yearly Report":
            break;
    }
    choiceIndex =mainOptions.IndexOf(choice);
} while (choice != "Exit");

//Console.WriteLine(choice);

