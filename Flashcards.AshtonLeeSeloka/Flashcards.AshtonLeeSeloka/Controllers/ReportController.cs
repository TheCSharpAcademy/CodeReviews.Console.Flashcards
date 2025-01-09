using Flashcards.AshtonLeeSeloka.Models;
using Flashcards.AshtonLeeSeloka.Views;
using FlashcardStack.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Services;
namespace Flashcards.AshtonLeeSeloka.Controllers;

internal class ReportController
{
	private readonly DataService _dataService = new DataService();
	private readonly UIViews _views = new UIViews();

	public void DisplayReport() 
	{
		
		List<StackModel> AvailableStacks = _dataService.GetAvailableStacks();
		StackModel selection = _views.SelectStackView(AvailableStacks, "Select [green]Stack[/] to [red]Generate Report[/]");
		List<int>? availableStackYears = _dataService.GetAvailableStackYears(selection.Stack_Name);
		int? selectedStackYear = _views.SelectStacYear(availableStackYears, "Select [green]Report[/] [red]Year[/]");
		if (selectedStackYear == null)
		{
			DisplayReport();
		}
		else 
		{
			List<Report> averageScorePerMonth = _dataService.PivotDateAverageSCore(selectedStackYear, selection.Stack_Name);
			List<Report> entriesPerMonth = _dataService.PivotDateCountEntries(selectedStackYear, selection.Stack_Name);
			_views.ReportView(averageScorePerMonth,entriesPerMonth);
		}
	}
}
