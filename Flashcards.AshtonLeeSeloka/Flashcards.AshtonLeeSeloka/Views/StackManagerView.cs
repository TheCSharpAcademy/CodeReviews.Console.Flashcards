using Spectre.Console;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;
namespace Flashcards.AshtonLeeSeloka.Views;

internal class StackManagerView
{
	public  ManageStacks MainMenu() 
	{
		Console.Clear();
		var Selection = AnsiConsole.Prompt(
		new SelectionPrompt<ManageStacks>()
			.Title("Select [green]Stack to manage[/]")
			.PageSize(10)
			.AddChoices(Enum.GetValues<ManageStacks>()));
		return Selection;
	}
}
