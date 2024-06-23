using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;

namespace FlashcardsProgram;

public class Application(StacksRepository stacksRepository, BaseRepository<FlashcardDAO> flashcardsRepository)
{

    // public FlashcardUI flashcardUI = new FlashcardUI(flashcardsRepository);

    public StackUI stackUI = new StackUI(stacksRepository, new FlashcardUI(flashcardsRepository));


    public void Start()
    {
        bool isAppRunning = true;

        ShowWelcomeMessage();
        do
        {
            isAppRunning = ShowMainMenu();
        } while (isAppRunning);

        ShowExitMessage();
    }

    public bool ShowMainMenu()
    {
        bool isRunning = true;

        var selected = Menu.ShowMainMenu();

        switch (selected)
        {
            case Menu.CREATE_SESSION:
                break;
            case Menu.VIEW_SESSIONS:
                break;
            case Menu.MANAGE_STACK:
                bool showManageStacksMenu = true;

                do
                {
                    var selectedStack = stackUI.SelectStackFromList();
                    if (selectedStack == null || selectedStack.Id == -1)
                    {
                        return true;
                    }

                    showManageStacksMenu = stackUI.ManageStack(selectedStack);
                } while (showManageStacksMenu);
                break;
            case Menu.CREATE_STACK:
                stackUI.CreateOrUpdateStack();
                break;
            case Menu.EXIT:
                isRunning = false;
                break;
        }

        if (isRunning)
        {
            Utils.ConsoleUtil.PressAnyKeyToClear();
        }

        return isRunning;
    }
    public static void ShowExitMessage()
    {
        Console.WriteLine("Bye!");
    }

    public static void ShowWelcomeMessage()
    {
        Console.WriteLine("Welcome");
    }

}