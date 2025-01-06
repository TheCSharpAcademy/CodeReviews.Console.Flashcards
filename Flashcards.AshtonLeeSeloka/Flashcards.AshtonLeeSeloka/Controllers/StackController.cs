using Flashcards.AshtonLeeSeloka.Views;

namespace Flashcards.AshtonLeeSeloka.Controllers;

internal class StackController
{
	private readonly StackManagerView _stackManagerView = new StackManagerView();
	public void MainMenu() 
	{
		var selection = _stackManagerView.MainMenu();
	
	
	}

}
