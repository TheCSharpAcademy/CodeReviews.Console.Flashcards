using Flashcards.CoreyJordan;

int width = 60;
FlashcardDisplay display = new(width);

display.WelcomeScreen();
display.PromptUser("");
display.MainMenu();

bool exitApp = false;
while (exitApp == false)
{
    string selection = Console.ReadLine()!;
	switch (selection.ToUpper())
	{
		case "N":
            exitApp = false;
			break;
		case "D":
			exitApp = false;
			break;
		case "R":
			exitApp = false;
            break;
		case "F":
			exitApp = false;
            break;
		case "Q":
			exitApp = true;
			break;
        default:
			display.PromptUser("Invalid choice, try again");
			display.MainMenu();
			exitApp = false;
			break;
	}
}