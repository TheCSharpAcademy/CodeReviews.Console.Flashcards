

using Flashcards.Bina28.DBmanager;
using Flashcards.Bina28.Models;
using Spectre.Console;


namespace Flashcards.Bina28.Controllers;

internal class StacksController
{
	StacksDB db = new();
	public List<StacksDto> stacks;
	private readonly Helper _inputHelper = new Helper();

	public StacksController()
	{
		stacks = db.GetAllRecords() ?? new List<StacksDto>();
	}

	// Create a table with a single column for stack names
	internal void ManageStacks()
	{
		var table = new Table();
		table.Border = TableBorder.Rounded;
		table.AddColumn("[#ff007f]Name[/]");

		foreach (StacksDto stack in stacks)
		{
			table.AddRow($"[yellow]{stack.Name}[/]");
		}
		AnsiConsole.Write(table);
	}

	// Prompts the user to input a valid stack name that exists in the list of stacks.
	// Ensures the input matches an existing stack.
	internal string InputStack()
	{
		string input;
		bool isInStack;
		do
		{
			input = _inputHelper.GetNonEmptyInput("\nEnter the name of the stack you want to study with: ");
			isInStack = stacks.Any(stack => stack.Name.Equals(input, StringComparison.OrdinalIgnoreCase));
		} while (!isInStack);

		return input;
	}

	// Creates a new stack with a name provided by the user.
	internal void CreateStack()
	{
		string stackName = _inputHelper.GetNonEmptyInput("Enter the name of stack you want to create: ");
		db.CreateStack(stackName);

		_inputHelper.WaitForKeyPress();
	}

	// Updates an existing stack's name.
	// Prompts the user for a new name to replace the current one.
	internal void UpdateStack(string stackName)
	{
		string newStackName = _inputHelper.GetNonEmptyInput($"The current stack name is '{stackName}'. Enter a new name to update it: ");
		db.UpdateStack(stackName, newStackName);

		_inputHelper.WaitForKeyPress();
	}

	// Deletes a stack after confirmation from the user
	internal void DeleteStack(string stackName)
	{
		if (ConfirmDeletion(stackName))
		{
			db.DeleteStack(stackName);
			Console.WriteLine($"Stack '{stackName}' has been deleted.");
		}
		else
		{
			Console.WriteLine($"Stack '{stackName}' was not deleted.");
		}
		_inputHelper.WaitForKeyPress();
	}

	// Asks the user for confirmation before deleting a stack.
	// Ensures valid input ('y' or 'n') is provided.
	internal bool ConfirmDeletion(string stackName)
	{
		string input;
		do
		{
			Console.WriteLine($"Are you sure you want to delete stack '{stackName}'? (y/n): ");
			input = Console.ReadLine()?.ToLower();
		}
		while (input != "y" && input != "n");

		return input == "y";
	}
}
