
using Flashcards.Bina28;
using Flashcards.Bina28.DBmanager;

try
{
	// Step 1: Establish the database connection
	// This method sets up a connection to the database for subsequent operations.
	DBConfig.EstablishConnection();

	// Step 2: Create and manage the operation status table
	// Initialize the OperationManager class to handle table creation.
	OperationManager operationalManager = new();
	operationalManager.CreateOperationStatusTable();

	// Step 3: Create and populate the stacks table
	// Initialize the StacksDB class to create the 'stacks' table and insert default values.
	StacksDB db = new();
	db.CreateStacksTable(); 
	db.InsertStacks();     

	// Step 4: Create and populate the flashcards table
	// Initialize the FlashcardsDB class to handle flashcards data.
	FlashcardsDB cardDb = new();
	cardDb.CreateFlashcardTable();      
	cardDb.InsertVocabularyFlashcards(); 

	// Step 5: Create a table for study session logs
	// Initialize the StudySessionDB class to manage study session log data.
	StudySessionDB studyDb = new();
	studyDb.CreateStudySessionLogTable(); 

	// Step 6: Launch the user interface
	// Initialize and display the main menu for user interaction.
	UserInterface userInterface = new();
	userInterface.MainMenu();
}
catch (Exception ex)
{
	// Error handling block
	// Catches any exceptions that occur during the program execution and outputs an error message.
	Console.WriteLine($"An error occurred: {ex.Message}");
}
