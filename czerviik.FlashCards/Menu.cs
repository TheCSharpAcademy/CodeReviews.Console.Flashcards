using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards;

public abstract class Menu
{
    protected MenuManager MenuManager { get; }
    protected FlashcardDb FlashcardDb { get; }
    protected StackDb StackDb { get; }
    protected Menu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb)
    {
        MenuManager = menuManager;
        FlashcardDb = flashcardDb;
        StackDb = stackDb;
    }
    public abstract void Display();
}
public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb) : base(menuManager, flashcardDb, stackDb) { }

    public override void Display()
    {
        UserInterface.MainMenu();

        switch (UserInterface.OptionChoice)
        {
            case "New Study Session":
                MenuManager.NewMenu(new StudySessionMenu(MenuManager, FlashcardDb, StackDb));
                break;
            case "New Flashcard":
                MenuManager.NewMenu(new FlashCardMenu(MenuManager, FlashcardDb, StackDb));
                break;
            case "Show Stacks":
                MenuManager.NewMenu(new ShowStacksMenu(MenuManager, FlashcardDb, StackDb));
                break;
            case "Show Study Sessions":
                break;
            default:
                Environment.Exit(0);
                break;
        }
    }
}

public class StudySessionMenu : Menu
{
    public StudySessionMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb) : base(menuManager, flashcardDb, stackDb) { }

    public override void Display()
    {
        UserInterface.StudySession();
    }
}

public class FlashCardMenu : Menu
{
    public FlashCardMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb) : base(menuManager, flashcardDb, stackDb) { }

    public override void Display()
    {
        var currentStack = "";
        var stacksList = StackDb.GetAll();
        var stacksArray = GetStackNamesArray(stacksList);
        
        UserInterface.NewFlashcard(stacksArray);

        switch (UserInterface.OptionChoice)
        {
            case "Create a new stack":
                break;
            case "Go back":
                MenuManager.GoBack();
                break;
            default:
                currentStack = UserInterface.OptionChoice;
                break;
        }

        UserInterface.NewFlashcardQuestion(currentStack);

    }
    private static string[] GetStackNamesArray(List<Stack> stacks)
    {
        var stacksArray = new string[stacks.Count];
        for (int i = 0; i < stacks.Count; i++)
        {
            stacksArray[0] = stacks[0].Name;
        }
        return stacksArray;
    }
}

public class ShowStacksMenu : Menu
{
    public ShowStacksMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb) : base(menuManager, flashcardDb, stackDb) { }

    public override void Display()
    {
        UserInterface.ShowStacks();
    }
}

public class ShowStudySessionsMenu : Menu
{
    public ShowStudySessionsMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb) : base(menuManager, flashcardDb, stackDb) { }

    public override void Display()
    {
        UserInterface.ShowStudySessions();
    }
}