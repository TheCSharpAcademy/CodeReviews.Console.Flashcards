
using Flashcards.Bina28;
using Flashcards.Bina28.DBmanager;



try
{
	// Establish the database connection
	DBConfig.EstablishConnection();

	// Create and insert into the stacks table
	StacksDB db = new();
	db.CreateStacksTable();
	db.InsertStacks();

	// Create and insert into the flashcards table
	FlashcardsDB cardDb = new();
	cardDb.CreateFlashcardTable();
	cardDb.InsertVocabularyFlashcards();

	// Create the study session log table
	StudySessionDB studyDb = new();
	studyDb.CreateStudySessionLogTable();

	// Start the user interface
	UserInterface userInterface = new();
	userInterface.MainMenu();
}
catch (Exception ex)
{
	// Error handling to catch any exceptions
	Console.WriteLine($"An error occurred: {ex.Message}");
}
