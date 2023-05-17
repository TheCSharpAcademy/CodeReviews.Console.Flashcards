using Flashcards.CoreyJordan;
using Flashcards.CoreyJordan.Consoles;

int width = 60;
FlashcardDisplay display = new(width);

display.WelcomeScreen();
display.PromptUser("");

bool exitApp = false;
while (exitApp == false)
{
	display.MainMenu();
    string selection = Console.ReadLine()!;
	switch (selection.ToUpper())
	{
		case "N":
			StudySessionConsole session = new(width);
			session.Study();
			break;
		case "D":
			DeckBuilderConsole deck = new(width);
			deck.ManageDecks();
			break;
		case "F":
			FlashCardBuilderConsole card = new(width);
			card.ManageCards();
            break;
		case "R":
            break;
		case "Q":
			exitApp = true;
			break;
        default:
			display.PromptUser("Invalid choice, try again");
			break;
	}
}