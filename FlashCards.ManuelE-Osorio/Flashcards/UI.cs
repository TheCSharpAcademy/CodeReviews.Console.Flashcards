namespace Flashcards;

class UI
{
    public bool RunFlashCardsProgram;
    public bool RunManageStacks;
    public bool RunManageFlashCards;
    public bool RunStudySession;
    
    public UI()
    {
        RunFlashCardsProgram = true;
        RunManageStacks = false;
        RunManageFlashCards = false;
        RunStudySession = false;
    }
    
    public void MainMenu()
    {
        do
        {
            Helpers.ClearConsole();
            Console.WriteLine(
            "1) Manage stacks\n"+
            "2) Manage Flashcards\n"+
            "3) Start a study session\n"+
            "4) View study sessions data\n");
            DataController.MainMenuSelection(Console.ReadLine());
        }
        while(RunFlashCardsProgram);
    }

    public void ManageStacks()
    {
        do
        {
            Helpers.ClearConsole();
            Console.WriteLine(
            "1) Create a new stack\n"+
            "2) Select a stack\n"+
            "3) Delete a stack\n");
            DataController.StackSelection(Console.ReadLine());
        }
        while(RunManageStacks);
    }

    public static void ManageFlashCards()
    {

    }
}