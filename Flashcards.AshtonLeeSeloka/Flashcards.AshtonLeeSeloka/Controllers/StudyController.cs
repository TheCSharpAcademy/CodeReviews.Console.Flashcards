using Flashcards.AshtonLeeSeloka.DTO;
using FlashcardStack.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Services;
using FlashcardStack.AshtonLeeSeloka.Views;
namespace FlashcardStack.AshtonLeeSeloka.Controllers;

internal class StudyController
{
	private readonly DataService _dataService = new DataService();
	private readonly StudyView _studyView = new StudyView();	
	public void StartStudying()
	{
		List<StackModel> AvailableStacks = _dataService.GetAvailableStacks();
		StackModel selection =_studyView.SelectStackView(AvailableStacks);
		List<CardDTO> cards = _dataService.GetCards(selection);
	}

	public void StudyStack(List<CardDTO> cards) 
	{
	
	}
}
