namespace Flashcards;

class DataController
{
    public static void MainMenuSelection(string? selection)
    {
        switch(selection)
        {
            case("1"):
                Flashcards.ProgramUI.RunManageStacks = true;
                Flashcards.ProgramUI.ManageStacks();
                break;
            case("2"):

                UI.ManageFlashCards();
                break;
            case("3"):
                break;
            case("4"):
                break;
            case("0"):
                Flashcards.ProgramUI.RunFlashCardsProgram = false;
                break;
            default:
                break;
        }
    }

    public static void StackSelection(string? selection)
    {
        switch(selection)
        {
            case("1"):
                break;
            case("0"):
                Flashcards.ProgramUI.RunManageStacks = false;
                break;
            default:
                break;
        }
    }
}