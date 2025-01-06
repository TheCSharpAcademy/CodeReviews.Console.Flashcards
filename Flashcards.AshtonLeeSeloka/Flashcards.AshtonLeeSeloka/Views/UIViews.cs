using FlashcardStack.AshtonLeeSeloka.MenuEnums;
using FlashcardStack.AshtonLeeSeloka.Models;
using Spectre.Console;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;
namespace Flashcards.AshtonLeeSeloka.Views;

internal class UIViews
{
	public MainMenu MainMenu()
	{
		Console.Clear();
		var Selection = AnsiConsole.Prompt(
		new SelectionPrompt<MainMenu>()
			.Title("Welcome to [green]FlashCards[/] make your selection")
			.PageSize(10)
			.AddChoices(Enum.GetValues<MainMenu>()));
		return Selection;
	}

	public ManageStacks ManageStacksMenu()
	{
		Console.Clear();
		var Selection = AnsiConsole.Prompt(
		new SelectionPrompt<ManageStacks>()
			.Title("Welcome to [green]FlashCards[/] make your selection")
			.PageSize(10)
			.AddChoices(Enum.GetValues<ManageStacks>()));
		return Selection;
	}

	public StackModel SelectStackView(List<StackModel> availableStacks,string text)
	{
		
		var StackSelection = AnsiConsole.Prompt(
			new SelectionPrompt<StackModel>()
			.Title(text)
			.UseConverter(s => $"{s.Stack_Name}")
			.AddChoices(availableStacks.ToList())
			);
		return StackSelection;
	}
}
