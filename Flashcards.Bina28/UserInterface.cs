
using Flashcards.Bina28.Controllers;

using Spectre.Console;


namespace Flashcards.Bina28;

internal class UserInterface
{
	internal static string stackName;
	private readonly FlashcardsController _flashCard_controller = new();
	private readonly StacksController _stacks_controller = new();
	private readonly StudyController _study_controller = new();
	private readonly Helper _inputHelper = new Helper();
	internal void MainMenu()
	{
		bool continueProgram = true;
		while (continueProgram)
		{
			Console.Clear();
			var actionList = new List<string> { "Manage Stacks", "Manage Flashcards", "Study", "View study session data", "Exit" };
			var actionChoice = AnsiConsole.Prompt(
				new SelectionPrompt<string>()
				.Title("Main menu:")
				.AddChoices(actionList));

			switch (actionChoice)
			{
				case "Manage Stacks":
					StackMenu();
					break;
				case "Manage Flashcards":
					FlashcardMenu();
					break;
				case "Study":
					StudyMenu();
					break;
				case "View study session data":
					_study_controller.ViewSession();
					_inputHelper.WaitForKeyPress();
					break;
				case "Exit":
					continueProgram = false;
					break;
				default:
					Console.WriteLine("Invalid input");
					break;
			}
		}
	}

	internal void StackMenu()
	{
		_stacks_controller.ManageStacks();
		stackName = _stacks_controller.InputStack();
		UsersStackmenu(stackName);

	}

	internal void UsersStackmenu(string stackName)
	{
		Console.Clear();
		Console.WriteLine($"You're currently working with {stackName} stack");
		Console.WriteLine("---------------------------------");
		Console.WriteLine("What do you want to do next: ");
		Console.WriteLine("0 - return to the main menu");
		Console.WriteLine("1 - create a new stack");
		Console.WriteLine("2 - edit stack's name");
		Console.WriteLine("3 - delete stack");
		Console.WriteLine("---------------------------------");

		int actionNumber;
		do
		{
			actionNumber = _inputHelper.GetValidIntegerInput("Please enter an action number (0-3): ");

		} while (actionNumber < 0 || actionNumber > 3);
		Console.Clear();
		switch (actionNumber)
		{
			case 0:
				return;
			case 1:
				_stacks_controller.CreateStack();
				break;
			case 2:
				_stacks_controller.UpdateStack(stackName);
				break;
			case 3:
				_stacks_controller.DeleteStack(stackName);
				break;
			default:
				Console.WriteLine("Invalid input");
				break;
		}
	}

	private void FlashcardMenu()
	{
		_stacks_controller.ManageStacks();
		stackName = _stacks_controller.InputStack();
		_flashCard_controller.RefreshFlashCards(UserInterface.stackName);
		Console.Clear();
		Console.WriteLine($"You're currently working with {stackName} stack");
		Console.WriteLine("---------------------------------");
		Console.WriteLine("What do you want to do next: ");
		Console.WriteLine("0 - return to the main menu");
		Console.WriteLine("1 - view X amount of Flashcards in the stack");
		Console.WriteLine("2 - view all Flashcards in the stack");
		Console.WriteLine("3 - create a new flashcard");
		Console.WriteLine("4 - edit a flashcard");
		Console.WriteLine("5 - delete a flashcard");
		Console.WriteLine("---------------------------------");


		int actionNumber;
		do
		{
			actionNumber = _inputHelper.GetValidIntegerInput("Please enter an action number (0-5): ");
		} while (actionNumber < 0 || actionNumber > 5);
		Console.Clear();
		switch (actionNumber)
		{
			case 0:
				return;
			case 1:
				_flashCard_controller.DisplayFlashcardCount();
				break;
			case 2:
				_flashCard_controller.DisplayAllFlashcards(stackName);
				_inputHelper.WaitForKeyPress();
				break;
			case 3:
				_flashCard_controller.AddFlashcardToStack();
				break;
			case 4:
				_flashCard_controller.UpdateFlashCard();
				break;
			case 5:
				_flashCard_controller.DeleteFlashCard();
				break;
			default:
				Console.WriteLine("Invalid input");
				break;
		}
	}

	public void StudyMenu()
	{
		_stacks_controller.ManageStacks();
		stackName = _stacks_controller.InputStack();
		_flashCard_controller.RefreshFlashCards(stackName);
		Console.Clear();
		Console.WriteLine($"You are working with the '{stackName}' stack\n");		
		_study_controller.StudySession();
	}
}

