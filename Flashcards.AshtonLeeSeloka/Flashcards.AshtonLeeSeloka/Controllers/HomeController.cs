using Flashcards.AshtonLeeSeloka.Services;
using Flashcards.AshtonLeeSeloka.Views;
using static Flashcards.AshtonLeeSeloka.MenuEnums.MenuEnums;
namespace Flashcards.AshtonLeeSeloka.Controllers;

internal class HomeController
{
	private readonly HomeView _homeView = new HomeView();

	public void Start() 
	{
		while (true) 
		{
			var selection =_homeView.MainMenu();

			switch (selection) 
			{
				case MainMenu.View_Reports:
					break;
				case MainMenu.Manage_Stacks:
					break;
				case MainMenu.Study:
					break;
				case MainMenu.Exit:
					Environment.Exit(0);
					break;
			}
		}
	}
}
