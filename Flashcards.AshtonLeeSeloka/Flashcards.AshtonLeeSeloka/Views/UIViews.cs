using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.MenuEnums;
using FlashcardStack.AshtonLeeSeloka.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Spectre.Console;
using System;
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

	public int? SelectStacYear(List<int>? availableStackYears, string text)
	{
		if (availableStackYears.IsNullOrEmpty())
		{
			Console.WriteLine($"No available study History for Selected Stack.\nPress any key to exit\n");
			return null;
		}

		var StackSelection = AnsiConsole.Prompt(
			new SelectionPrompt<int>()
			.Title(text)
			.AddChoices(availableStackYears.ToList())
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
		Console.Clear();
		var selection = AnsiConsole.Prompt(
			new SelectionPrompt<CardDTO>()
			.Title(message)
			.UseConverter((c) =>$"Front: {c.Front}")
			.AddChoices(cards.ToList())
			);
		return selection;
	}

	public StudyOptions StudyOptions() 
	{
		Console.Clear();
		var Selection = AnsiConsole.Prompt(
		new SelectionPrompt<StudyOptions>()
			.Title("[green]Select Study Option[/]")
			.PageSize(10)
			.AddChoices(Enum.GetValues<StudyOptions>()));
		return Selection;
	}

	public string QuestionAnswer(List<string> questions) 
	{
		var Selection = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
			.Title("Select [green]Correct[/] Answer")
			.PageSize(10)
			.AddChoices(questions));
		return Selection;
	}

	public void DisplayCardFront(string front) 
	{
		var table = new Table();
		table.AddColumn("[cyan]Front[/]");
		table.AddRow($"{front}");
		table.Border(TableBorder.Rounded);
		AnsiConsole.Write(table);
	}

	public void ReportView(List<Report> averageScorePerMonth, List<Report> entriesPerMonth) 
	{
		Console.Clear();
		AnsiConsole.WriteLine("Average Score per month for selected year");
		ReportTable(averageScorePerMonth);
		AnsiConsole.WriteLine("\nEntries per month for selected year");
		ReportTable(entriesPerMonth);
		AnsiConsole.WriteLine("\n Press any Key to Exit");
		Console.ReadLine();
	}

	public void ReportTable(List<Report> reportValues) 
	{
		var table = new Table();
		table.AddColumn("[cyan]Stack[/]");
		table.AddColumn("[yellow]Jan[/]");
		table.AddColumn("[yellow]Feb[/]");
		table.AddColumn("[yellow]Mar[/]");
		table.AddColumn("[yellow]Apr[/]");
		table.AddColumn("[yellow]May[/]");
		table.AddColumn("[yellow]Jun[/]");
		table.AddColumn("[yellow]Jul[/]");
		table.AddColumn("[yellow]Aug[/]");
		table.AddColumn("[yellow]Sep[/]");
		table.AddColumn("[yellow]Oct[/]");
		table.AddColumn("[yellow]Nov[/]");
		table.AddColumn("[yellow]Dec[/]");

		foreach (Report report in reportValues)
		{
			table.AddRow($"{report.Stack}", $"{Math.Round((decimal)report.Jan)}", $"{Math.Round((decimal)report.Feb)}", $"{Math.Round((decimal)report.Mar)}", $"{Math.Round((decimal)report.Apr)}", $"{Math.Round((decimal)report.May)}", $"{Math.Round((decimal)report.Jun)}", $"{Math.Round((decimal)report.Jul)}", $"{Math.Round((decimal)report.Aug)}", $"{Math.Round((decimal)report.Sep)}", $"{Math.Round((decimal)report.Oct)}", $"{Math.Round((decimal)report.Nov)}", $"{Math.Round((decimal)report.Dec)}");
		}
		table.Border(TableBorder.Rounded);
		AnsiConsole.Write(table);
	}
}
