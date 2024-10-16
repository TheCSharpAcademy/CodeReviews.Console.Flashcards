namespace FlashCardsApp;

using FlashCards;
using FlashCardsLibrary;
using FlashCardsLibrary.Models;
using Spectre.Console;
using System.Xml.Linq;

public class Program
{
    public static void Main()
    {

        MenuManager.Greeting();
        Task task;
        try
        {
            task = new Task(() => Database.InitialiseDB());
            task.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Initialising Database {e}");
            return;
        }
        task.Wait();
        MenuManager.EnterPause();
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            MenuManager.MainMenu();
            switch (InputManager.GetOption(1, 5))
            {
                case 1:
                    MenuManager.MakeReport();
                    //TODO: Make Reports
                    ReportBuilder.MakeReport();
                    break;
                case 2:
                    ManageStacks();
                    break;
                case 3:
                    MenuManager.MakeStudySession();
                    StudySessionBuilder.StartSession();
                    break;
                case 4:
                    MenuManager.HelpMenu();
                    break;
                case 5:
                    isRunning = false;
                    break;
            }

        }
    }
    public static void ManageStacks()
    {
        MenuManager.ManageStacksMenu();
        List<Stack> stacks = StackController.GetStackNames();
        switch (InputManager.GetOption(1, 4,true))
        {
            case 1:
                var name = InputManager.NewStack();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    StackController.AddStack(new Stack(name));
                    int cardsAmount = InputManager.GetOption(0, 10, true,quantity: "Cards amount");

                    
                    for (int i = 0; i < cardsAmount; i++)
                    {
                        var card = InputManager.NewFlashCard(new Stack(name));
                        FlashCardController.AddFlashCard(card);
                    }
                }
                break;
            case 2:
                var oldStack = stacks[InputManager.GetOption(1, stacks.Count,quantity:"Stack No. ") - 1];
                var newStack = new Stack(InputManager.NewStack());
                if (!string.IsNullOrWhiteSpace(newStack.Name))
                    StackController.UpdateStack(oldStack, newStack);
                break;
            case 3:
                var stack = stacks[InputManager.GetOption(1, stacks.Count,quantity:"Stack No. ") - 1];
                StackController.DeleteStack(stack);
                break;
            case 4:
                //Edit Stack FlashCards
                stack = stacks[InputManager.GetOption(1, stacks.Count, quantity: "Stack No. ") - 1];
                EditFlashCards(stack);

                break;
        }
    }
    public static void EditFlashCards(Stack stack)
    {
        var cards = MenuManager.EditFlashCardsMenu(stack);
        switch (InputManager.GetOption(1,3,true))
        {
            case 1:
                //add
                int cardsAmount = InputManager.GetOption(1,10,quantity:"Cards amount");

                FlashCardCreate card;
                for (int i = 0; i < cardsAmount; i++)
                {
                    card = InputManager.NewFlashCard(stack);
                    FlashCardController.AddFlashCard(card);
                }
               
                break;
            case 2:
                //edit
                card = InputManager.NewFlashCard(stack);
                FlashCardUpdate newCard = new FlashCardUpdate(cards[InputManager.GetOption(1,cards.Count,quantity:"Card ID")-1].ID,card.Front,card.Back);
                FlashCardController.UpdateFlashCard(newCard);
                break;
            case 3:
                //delete
                FlashCardController.DeleteFlashCard(cards[InputManager.GetOption(1, cards.Count, quantity: "Card ID") - 1].ID);
                break;
        }
    }
}
