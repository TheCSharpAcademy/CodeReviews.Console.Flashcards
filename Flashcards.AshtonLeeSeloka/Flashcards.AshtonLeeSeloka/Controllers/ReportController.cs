using Flashcards.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Services;

namespace Flashcards.AshtonLeeSeloka.Controllers;

internal class ReportController
{
	private readonly DataService _dataService = new DataService();

	public void DisplayReport() 
	{
		List<Report> row = _dataService.PivotDateAverageSCore(2025,"German");

	}
	
}
