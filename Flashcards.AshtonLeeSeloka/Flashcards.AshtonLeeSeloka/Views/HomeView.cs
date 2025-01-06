namespace FlashcardStack.AshtonLeeSeloka.Views;
using Spectre.Console;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;

internal class HomeView
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
}
