


using Flashcards.Bina28.DBmanager;
using Flashcards.Bina28.Models;
using Spectre.Console;

namespace Flashcards.Bina28.Controllers;
internal class FlashcardsController
{
	private readonly FlashcardsDB _db;
	private List<FlashCardsDto> _flashCards;
	private readonly Helper _inputHelper = new Helper();

	public FlashcardsController()
	{
		_db = new FlashcardsDB();
		LoadFlashCards();
	}

	internal void LoadFlashCards()
	{
		if (string.IsNullOrEmpty(UserInterface.stackName))
		{
			_flashCards = new List<FlashCardsDto>();
			return;
		}

		_flashCards = _db.GetAllRecords(UserInterface.stackName) ?? new List<FlashCardsDto>();
	}

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

	public void RefreshFlashCards(string stackName)
	{
		LoadFlashCards();
	}
	internal void DisplayFlashcardCount()
	{
		int numberOfCards = 0;
		do
		{
			numberOfCards = _inputHelper.GetValidIntegerInput("Enter the amount of flashcards you want to disply (must be be less than or equal to the total number of cards): ");
			if (_flashCards.Count == 0)
			{
				Console.WriteLine("There are no available cards in the stack");
				return;
			}
		}
		while (numberOfCards > _flashCards.Count);
		CreateTable(numberOfCards);
		_inputHelper.WaitForKeyPress();

	}

	internal void AddFlashcardToStack()
	{
		Console.WriteLine($"Please enter the following details to create a new flashcard in the '{UserInterface.stackName}' stack:");
		string question = _inputHelper.GetNonEmptyInput("Enter a question: ");
		string answer = _inputHelper.GetNonEmptyInput("Enter an answer: ");
		_db.CreateCard(question, answer, UserInterface.stackName);
		Console.WriteLine($"The question: {question} and answer: {answer} were successfully added");
		_inputHelper.WaitForKeyPress();

	}

	internal void UpdateFlashCard()
	{
		DisplayAllFlashcards(UserInterface.stackName);
		int number = _inputHelper.GetValidIntegerInput("Enter the number of flachcard which you want to update: ");
		if (!isCardExist(number))
		{
			return;
		}

		FlashCardsDto selectedFlashcard = _flashCards[number - 1];
		string question = _inputHelper.GetNonEmptyInput("Enter a new question: ");
		string answer = _inputHelper.GetNonEmptyInput("Enter an new answer: ");
		_db.UpdateCard(question, answer, (int)selectedFlashcard.Flashcard_id, UserInterface.stackName);
		_inputHelper.WaitForKeyPress();
	}

	internal void DeleteFlashCard()
	{
		DisplayAllFlashcards(UserInterface.stackName);
		int number = _inputHelper.GetValidIntegerInput("Enter the number of flachcard you want to delete: ");

		if (!isCardExist(number))
		{
			return;
		}
		FlashCardsDto selectedFlashcard = _flashCards[number - 1];
		_db.DeleteCard((int)selectedFlashcard.Flashcard_id);
		Console.WriteLine($"The flashcard was successfully deleted");
		_inputHelper.WaitForKeyPress();
	}

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

	internal bool IsCardExist(int number)
	{
		if (number < 1 || number > _flashCards.Count)
		{
			Console.WriteLine("Invalid flashcard number. Please try again.");
			return false;
		}
		return true;
	}


	public List<FlashCardsDto> GetFlashCards()
	{
		return _flashCards;
	}

}
