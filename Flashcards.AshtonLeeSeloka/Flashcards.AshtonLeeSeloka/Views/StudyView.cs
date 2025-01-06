using FlashcardStack.AshtonLeeSeloka.Models;
using Spectre.Console;

namespace FlashcardStack.AshtonLeeSeloka.Views;

internal class StudyView
{
	public StackModel SelectStackView(List<StackModel> availableStacks) 
	{
		var StackSelection = AnsiConsole.Prompt(
			new SelectionPrompt<StackModel>()
			.Title("Select the [green]Stack[/] to study")
			.UseConverter(s => $"{s.Stack_Name}")
			.AddChoices(availableStacks.ToList())
			);
		return StackSelection;
	}
}
