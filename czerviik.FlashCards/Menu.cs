using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashCards;

public abstract class Menu
{
    protected MenuManager MenuManager { get; }
    protected Menu(MenuManager menuManager)
    {
        MenuManager = menuManager;
    }
    public abstract void Display();
}
public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.MainMenu();

        switch (UserInterface.OptionChoice)
        {
            case "New Study Session":
                MenuManager.NewMenu(new StudySessionMenu(MenuManager));
                break;
            case "New Flashcard":
                MenuManager.NewMenu(new FlashCardMenu(MenuManager));
                break;
            case "Show Stacks":
                MenuManager.NewMenu(new ShowStacksMenu(MenuManager));
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
    public StudySessionMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.StudySession();
    }
}

public class FlashCardMenu : Menu
{
    public FlashCardMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.NewFlashCard();
    }
}

public class ShowStacksMenu : Menu
{
    public ShowStacksMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.ShowStacks();
    }
}

public class ShowStudySessionsMenu : Menu
{
    public ShowStudySessionsMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.ShowStudySessions();
    }
}