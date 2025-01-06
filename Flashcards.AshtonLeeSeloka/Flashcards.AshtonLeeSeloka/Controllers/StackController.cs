using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Views;
using FlashcardStack.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Services;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;

namespace Flashcards.AshtonLeeSeloka.Controllers;

internal class StackController
{
	private readonly DataService _dataService = new DataService();
	private readonly UIViews _views = new UIViews();
	
	public void MainMenu()
	{
		var selection = _views.ManageStacksMenu();
		switch (selection) 
		{
			case ManageStacks.Create_New_Stack:
				break;
			case ManageStacks.Manage_Existing_Stacks:
				ManageExistingStack();
				break;
			case ManageStacks.Exit:
				break;
		}
	}

	public void ManageExistingStack() 
	{
		List<StackModel> AvailableStacks = _dataService.GetAvailableStacks();
		StackModel selection = _views.SelectStackView(AvailableStacks, "Select the [green]Stack[/] to Manage");
		List<CardDTO> cards = _dataService.GetCards(selection);
	}

	public void CreateNewStack() 
	{
	
	}
}
