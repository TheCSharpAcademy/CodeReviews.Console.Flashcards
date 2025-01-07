using Flashcards.AshtonLeeSeloka.Controllers;
using Flashcards.AshtonLeeSeloka.Views;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;
namespace FlashcardStack.AshtonLeeSeloka.Controllers;

internal class HomeController
{
	private readonly UIViews _view = new UIViews();
	private readonly StudyController _studyController = new StudyController();
	private readonly StackController _stackController = new StackController();

	public void Start()
	{
		while (true)
		{
			var selection = _view.MainMenu();

			switch (selection)
			{
				case MainMenu.View_Reports:
					break;
				case MainMenu.Manage_Stacks:
					_stackController.MainMenu();
					break;
				case MainMenu.Study:
					_studyController.StartStudying();
					break;
				case MainMenu.Exit:
					Environment.Exit(0);
					break;
			}
		}
	}
}
