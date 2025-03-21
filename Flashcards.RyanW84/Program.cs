using Flashcards.RyanW84;

var dataAccess = new DataAccess();

dataAccess.ConfirmConnection();

dataAccess.CreateTables();

//Seed Data method goes here, when its needed

UserInterface.MainMenu();
