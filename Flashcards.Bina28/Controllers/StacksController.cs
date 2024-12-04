

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

	internal void CreateStack()
	{
		string stackName = _inputHelper.GetNonEmptyInput("Enter the name of stack you want to create: ");
		Console.WriteLine($"The stack '{stackName}' was successfullt created");
		db.CreateStack(stackName);
		_inputHelper.WaitForKeyPress();
	}

	internal void UpdateStack(string stack)
	{
		string newStackName = _inputHelper.GetNonEmptyInput($"The current stack name is '{stack}'. Enter a new name to update it: ");
		db.UpdateStack(stack, newStackName);
		_inputHelper.WaitForKeyPress();
	}

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
