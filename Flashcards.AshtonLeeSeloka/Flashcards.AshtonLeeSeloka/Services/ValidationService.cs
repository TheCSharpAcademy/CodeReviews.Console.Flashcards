using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Views;

namespace Flashcards.AshtonLeeSeloka.Services;

internal class ValidationService
{
	private readonly UIViews _view = new UIViews();
	public int getIndex(List<CardDTO> cards) 
	{
		while (true) 
		{
			string cardIDInput = _view.PromptUser("Enter ID of card to Edit or enter 0 to exit");
			if (cardIDInput == "0")
				return 0;

			bool validInput = int.TryParse(cardIDInput, out _);
			if (validInput) 
			{
				if (int.Parse(cardIDInput)<=cards.Count) 
					return int.Parse(cardIDInput); 
				else
					continue;
			}
			else 
			{
				continue;
			}
		}
	}
}
