


using Flashcards.Bina28.DBmanager;
using Flashcards.Bina28.Models;
using Spectre.Console;

namespace Flashcards.Bina28.Controllers;
internal class FlashcardsController
{
	private readonly FlashcardsDB _db; // Database access object for flashcards
	private List<FlashCardsDto> _flashCards; // Local cache of flashcards
	private readonly Helper _inputHelper = new Helper(); // Helper class for user input handling

	public FlashcardsController()
	{
		_db = new FlashcardsDB();
		LoadFlashCards();
	}

	// Loads all flashcards for the current stack name into memory.
	// If no stack name is selected, initializes an empty list.
	internal void LoadFlashCards()
	{
		_flashCards = string.IsNullOrEmpty(UserInterface.stackName)
				? new List<FlashCardsDto>()
				: _db.GetAllRecords(UserInterface.stackName) ?? new List<FlashCardsDto>();
	}

	// Displays all flashcards in the current stack.
	// Shows a table of flashcards or an empty message if no cards exist.
	internal void DisplayAllFlashcards(string stackName)
	{
		if (_flashCards.Count == 0)
		{
			Console.WriteLine("No flashcards available for the selected stack.");
			_inputHelper.WaitForKeyPress();
			return;
		}
		else
		{
			CreateTable(_flashCards.Count);
		}
	}

	
	// Reloads the flashcards by fetching updated data from the database.	
	public void RefreshFlashCards()
	{
		LoadFlashCards();
	}

	// Allows the user to display a specific number of flashcards.
	// Ensures the number entered is valid and does not exceed the total flashcards.
	internal void DisplayFlashcardCount()
	{
		if (IsFlashcardStackEmpty())
                return;

		int count = GetValidFlashcardCount(); // Get user input for the count of flashcards
            Console.Clear();
            CreateTable(count);
            _inputHelper.WaitForKeyPress();
	}

	// Adds a new flashcard to the current stack with user-provided question and answer.
	internal void AddFlashcardToStack()
	{
		Console.WriteLine($"Please enter the following details to create a new flashcard in the '{UserInterface.stackName}' stack:");

		string question = _inputHelper.GetNonEmptyInput("Enter a question: ");
		string answer = _inputHelper.GetNonEmptyInput("Enter an answer: ");

		_db.CreateCard(question, answer, UserInterface.stackName);

		RefreshFlashCards();
		_inputHelper.WaitForKeyPress();
	}

	// Updates an existing flashcard's question and answer.
	// Prompts the user to input the flashcard to update.
	internal void UpdateFlashCard()
	{
		DisplayAllFlashcards(UserInterface.stackName);

		if (IsFlashcardStackEmpty())
			return;

		string flashcardName = _inputHelper.GetNonEmptyInput("Enter the word (question) you want to update: ");
		var selectedFlashcard = FindFlashcardByName(flashcardName); // Find the flashcard

		if (selectedFlashcard == null)
			return;

		// Get new question and answer from user
		string newQuestion = _inputHelper.GetNonEmptyInput("Enter a new question: ");
		string newAnswer = _inputHelper.GetNonEmptyInput("Enter a new answer: ");

		// Update flashcard in database
		_db.UpdateCard(newQuestion, newAnswer, (int)selectedFlashcard.Flashcard_id, UserInterface.stackName);
		Console.WriteLine("Flashcard successfully updated!");

		RefreshFlashCards(); // Reload flashcards after updating
		_inputHelper.WaitForKeyPress();
	}

	// Deletes a flashcard by its question.
	// Prompts the user for the flashcard to delete.
	internal void DeleteFlashCard()
	{
		DisplayAllFlashcards(UserInterface.stackName);

		if (IsFlashcardStackEmpty())
			return;

		string flashcardName = _inputHelper.GetNonEmptyInput("Enter the word (question) you want to delete: ");
		var selectedFlashcard = FindFlashcardByName(flashcardName); // Find the flashcard

		if (selectedFlashcard == null)
			return;

		_db.DeleteCard((int)selectedFlashcard.Flashcard_id); // Delete from database
		Console.WriteLine("Flashcard successfully deleted!");

		RefreshFlashCards(); // Reload flashcards after deleting
		_inputHelper.WaitForKeyPress();
	}

	// Displays a table of flashcards.
	internal void CreateTable(int count)
	{
		var table = new Table
		{
			Border = TableBorder.Rounded
		};

		table.AddColumn("[#ff007f]Id[/]");
		table.AddColumn("[#ff007f]Question[/]");
		table.AddColumn("[#ff007f]Answer[/]");

		for (int i = 0; i < count; i++)
		{
			table.AddRow($"[yellow]{i + 1}[/]", $"[yellow]{_flashCards[i].Question}[/]", $"[yellow]{_flashCards[i].Answer}[/]");
		}
		AnsiConsole.Write(table);
	}

	// Ensures the flashcard stack is not empty before proceeding.
	// Displays a message and waits for user input if the stack is empty.	
	private bool IsFlashcardStackEmpty()
	{
		if (_flashCards.Count == 0)
		{
			Console.WriteLine("No flashcards available for the selected stack.");
			_inputHelper.WaitForKeyPress();
			return true;
		}
		return false;
	}

	
	// Gets a valid integer input from the user for the number of flashcards to display.
	// Ensures the number is within a valid range.
	private int GetValidFlashcardCount()
	{
		int count;
		do
		{
			count = _inputHelper.GetValidIntegerInput($"Enter the number of flashcards to display (max {_flashCards.Count}): ");
		}
		while (count > _flashCards.Count || count < 1);

		return count;
	}

	// Finds a flashcard by its question.
	// Displays an error message if the flashcard is not found.
	
	private FlashCardsDto FindFlashcardByName(string name)
	{
		var flashcard = _flashCards.FirstOrDefault(f => f.Question.Equals(name, StringComparison.OrdinalIgnoreCase));
		if (flashcard == null)
		{
			Console.WriteLine("Flashcard not found.");
			_inputHelper.WaitForKeyPress();
		}
		return flashcard;
	}

	public List<FlashCardsDto> GetFlashCards()
	{
		return _flashCards;
	}

	internal bool IsCardExist(int number)
	{
		if (number < 1 || number > _flashCards.Count)
		{
			Console.WriteLine("Invalid flashcard number. Please try again.");
			return false;
		}
		return true;
	}
}
