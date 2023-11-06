using Kmakai.FlashCards.Controllers;
using Kmakai.FlashCards.Data;
using Kmakai.FlashCards.Models;

namespace Kmakai.FlashCards;

public class FlashCardsApp
{
    private DbContext DbContext { get; set; } = new DbContext();
    public List<StudySession> StudySessions { get; set; } = new List<StudySession>();
    public List<Stack> Stacks { get; set; } = new List<Stack>();
    public Stack? CurrentStack { get; set; } = null;
    public List<Flashcard> StackFlashcards { get; set; } = new List<Flashcard>();
    public bool IsRunning { get; set; } = true;

    private MenuController MenuController { get; set; }

    public FlashCardsApp()
    {
        MenuController = new MenuController(this);
        DbContext.InitializeDatabase();
        Stacks = StackController.GetStacks();
        StudySessions = StudySessionController.GetSessions();
    }

    public void Start()
    {
        while (IsRunning)
        {
            DisplayController.DisplayMainMenu();
            MenuController.HandleMainMenu();
        }
    }


   

}
