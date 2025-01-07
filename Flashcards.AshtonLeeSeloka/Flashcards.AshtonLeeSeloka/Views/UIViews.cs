using Flashcards.AshtonLeeSeloka.DTO;
using FlashcardStack.AshtonLeeSeloka.MenuEnums;
using FlashcardStack.AshtonLeeSeloka.Models;
using Microsoft.VisualBasic;
using Spectre.Console;
using System.Collections.ObjectModel;
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

	public ManageExistingStack ManageExistingStacksMenu(StackModel selection)
	{
		var Selection = AnsiConsole.Prompt(
		new SelectionPrompt<ManageExistingStack>()
			.Title($"\nManage Stack [red]{selection.Stack_Name}[/] make your selection")
			.PageSize(10)
			.AddChoices(Enum.GetValues<ManageExistingStack>()));
		return Selection;
	}

	public StackModel SelectStackView(List<StackModel> availableStacks,string text)
	{
		
		var StackSelection = AnsiConsole.Prompt(
			new SelectionPrompt<StackModel>()
			.Title(text)
			.UseConverter((s) => $"{s.Stack_Name}")
			.AddChoices(availableStacks.ToList())
			);
		return StackSelection;
	}

	public void ViewCardsAsTable(List<CardDTO> cards) 
	{
		Console.Clear();
		var table = new Table();
		table.AddColumn("[cyan]ID[/]");
		table.AddColumn("[yellow]FRONT[/]");
		table.AddColumn("[green]Back[/]");

		foreach (CardDTO card in cards) 
		{
			table.AddRow($"{cards.IndexOf(card)+1}",$"{card.Front}",$"{card.Back}");
		}
		table.Border(TableBorder.Rounded);
		AnsiConsole.Write(table);
	}

	public String PromptUser(string prompt) 
	{
		var answer = AnsiConsole.Ask<string>(prompt);
		return answer;
	}

	public CardDTO selectSpecificCard(List<CardDTO>? cards,string message) 
	{
		int i = 0;
		var selection = AnsiConsole.Prompt(
			new SelectionPrompt<CardDTO>()
			.Title(message)
			.UseConverter((c) =>$"ID:{i++}, Front: {c.Front}, Back: {c.Back}")
			.AddChoices(cards.ToList())
			);
		return selection;
	}
}
