using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace FlashCards;

public abstract class Menu
{
    protected MenuManager MenuManager { get; }
    protected FlashcardDb FlashcardDb { get; }
    protected StackDb StackDb { get; }
    protected StudySessionDb SessionDb { get; }
    protected Menu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb)
    {
        MenuManager = menuManager;
        FlashcardDb = flashcardDb;
        StackDb = stackDb;
        SessionDb = sessionDb;
    }
    public abstract void Display();
}
public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb) : base(menuManager, flashcardDb, stackDb, sessionDb) { }

    public override void Display()
    {
        UserInterface.MainMenu();

        switch (UserInterface.OptionChoice)
        {
            case "New Study Session":
                MenuManager.NewMenu(new StudySessionMenu(MenuManager, FlashcardDb, StackDb, SessionDb));
                break;
            case "New Flashcard":
                MenuManager.NewMenu(new FlashCardMenu(MenuManager, FlashcardDb, StackDb, SessionDb));
                break;
            case "Show Stacks":
                MenuManager.NewMenu(new ShowStacksMenu(MenuManager, FlashcardDb, StackDb, SessionDb));
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
    public StudySessionMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb) : base(menuManager, flashcardDb, stackDb, sessionDb) { }
    private List<Stack> _stacksList;

    public override void Display()
    {
        _stacksList = StackDb.GetAll();

        if (_stacksList.Count != 0)
        {
            DisplayStackOptions();

            try
            {
                HandleUserOptions();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                UserInput.DisplayMessage();
                MenuManager.GoBack();
            }
        }
        else
        {
            UserInput.DisplayMessage("No stack yet.", "return to Main menu", true);
            MenuManager.ReturnToMainMenu();
        }
    }

    private void DisplayStackOptions()
    {
        var stacksArray = Operations.StackListToNamesArray(_stacksList);
        UserInterface.StudySessionOptions(stacksArray);
    }

    private void HandleUserOptions()
    {
        Stack userStack;

        switch (UserInterface.OptionChoice)
        {
            case "Go back":
                MenuManager.GoBack();
                break;
            default:
                userStack = _stacksList.FirstOrDefault(s => s.Name == UserInterface.OptionChoice);
                if (userStack == null)
                {
                    throw new InvalidOperationException($"A stack with the name {UserInterface.OptionChoice}, does not exist in this context.");
                }
                else
                {
                    MenuManager.NewMenu(new NewStudySession(MenuManager, FlashcardDb, StackDb, SessionDb, userStack));
                }
                break;
        }
    }
}


public class NewStudySession : Menu
{
    private Stack _userStack;
    private DateTime _startDateTime;
    private int _totalRounds;
    public NewStudySession(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb, Stack userStack) : base(menuManager, flashcardDb, stackDb, sessionDb)
    {
        _userStack = userStack;
    }

    public override void Display()
    {
        SetDateTime();
        StartSession(GetFlashcardSesDtos());
        FinalizeSession();
        
        if (RepeatSession())
            MenuManager.DisplayCurrentMenu();
        else
            MenuManager.ReturnToMainMenu();
    }
    private void SetDateTime()
    {
        _startDateTime = new DateTime();
        _startDateTime = DateTime.Now;
    }

    private void StartSession(List<FlashcardSessionDto> flashcardSesDtos)
    {
        _totalRounds = flashcardSesDtos.Count;
        for (int i = 0; i < _totalRounds; i++)
        {
            var round = new SessionRound(flashcardSesDtos, _userStack, MenuManager, _totalRounds);
            round.Start();
        }
    }

    private void FinalizeSession()
    {
        UserInterface.FinalizeSession(SessionRound.Score, _totalRounds, _userStack);
        SessionDb.Insert(_startDateTime, SessionRound.Score, _totalRounds, _userStack.Id);
        SessionRound.Reset();
    }

    private List<FlashcardSessionDto> GetFlashcardSesDtos()
    {
        var flashcards = FlashcardDb.GetByStackId(_userStack.Id);
        return Operations.ConvertToSessionDto(flashcards);
    }

    private bool RepeatSession()
    {
        return UserInterface.OptionChoice == "Yes";
    }
}
public class FlashCardMenu : Menu
{
    protected List<Stack> stacksList;
    public FlashCardMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb) : base(menuManager, flashcardDb, stackDb, sessionDb) { }

    public override void Display()
    {
        stacksList = StackDb.GetAll();

        if (stacksList.Count != 0)
        {
            DisplayStackOptions();

            try
            {
                HandleUserOptions();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                UserInput.DisplayMessage();
                MenuManager.GoBack();
            }
        }
        else
        {
            var newStack = CreateStack();
            HandleFlashcardCreation(newStack);
        }
    }

    private void DisplayStackOptions()
    {
        var stacksArray = Operations.StackListToNamesArray(stacksList);
        UserInterface.NewFlashcard(stacksArray);
    }

    private void HandleUserOptions()
    {
        Stack userStack;

        switch (UserInterface.OptionChoice)
        {
            case "Create a new stack":
                userStack = CreateStack();
                HandleFlashcardCreation(userStack);
                break;
            case "Go back":
                MenuManager.GoBack();
                break;
            default:
                userStack = stacksList.FirstOrDefault(s => s.Name == UserInterface.OptionChoice);
                if (userStack == null)
                {
                    throw new InvalidOperationException($"A stack with the name {UserInterface.OptionChoice}, does not exist in this context.");
                }
                else
                {
                    HandleFlashcardCreation(userStack);
                }
                break;
        }
    }

    private void HandleFlashcardCreation(Stack currentStack)
    {
        var anotherFlashcard = true;

        while (anotherFlashcard)
        {
            UserInterface.NewFlashcardQuestion(currentStack.Name);
            var userQuestion = UserInput.InputWithSpecialKeys(MenuManager, true, 50);

            UserInterface.NewFlashcardAnswer(currentStack.Name, userQuestion);
            var userAnswer = UserInput.InputWithSpecialKeys(MenuManager, true, 50);

            UserInterface.NewFlashcardConfirm(currentStack.Name, userQuestion, userAnswer);
            if (UserInterface.OptionChoice == "Confirm")
            {
                FlashcardDb.Insert(userQuestion, userAnswer, currentStack.Id);
                UserInterface.AnotherFlashcard();
                if (UserInterface.OptionChoice == "Done")
                {
                    anotherFlashcard = false;
                }
            }
        }
        MenuManager.DisplayCurrentMenu();
    }

    private Stack CreateStack()
    {
        string stackName = HandleStackNameInput();

        StackDb.Insert(stackName.ToLower());

        return StackDb.GetByName(stackName);
    }

    private string HandleStackNameInput()
    {
        string stackName;
        do
        {
            UserInterface.NewStack();
            stackName = UserInput.InputWithSpecialKeys(MenuManager, true, 50).ToLower();
            if (StackDb.NamePresent(stackName))
            {
                UserInput.DisplayMessage($"Stack {stackName} already exists.", "enter again");
            }
        } while (StackDb.NamePresent(stackName));

        return stackName;
    }
}

public class ShowStacksMenu : Menu
{

    protected List<Stack> stacksList;
    protected List<Flashcard> flashcards;
    protected List<FlashcardReviewDto> flashcardDtos;
    protected Stack userStack;
    public ShowStacksMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb) : base(menuManager, flashcardDb, stackDb, sessionDb) { }

    public override void Display()
    {
        stacksList = StackDb.GetAll();

        if (stacksList.Count != 0)
        {
            DisplayStackOptions();

            try
            {
                HandleUserOptions();
                HandleActionMenu();

            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                UserInput.DisplayMessage();
                MenuManager.GoBack();
            }
        }
        else
        {
            UserInput.DisplayMessage("No flashcards yet.", "return to Main Menu", true);
            MenuManager.ReturnToMainMenu();
        }
    }

    private void DisplayStackOptions()
    {
        var stacksArray = Operations.StackListToNamesArray(stacksList);

        UserInterface.ShowStacks(stacksArray);

    }

    private void DisplayFlashcards(Stack stack)
    {
        flashcardDtos = Operations.ConvertToDto(flashcards);
        UserInterface.ShowFlashcards(flashcardDtos, stack);
    }
    private void DisplayFlashcards(List<Stack> stacks)
    {
        flashcardDtos = Operations.ConvertToDto(flashcards);
        UserInterface.ShowFlashcards(flashcardDtos, stacks);
    }

    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case "Show all":
                flashcards = FlashcardDb.GetAll();
                DisplayFlashcards(stacksList);
                break;

            case "Go back":
                MenuManager.GoBack();
                break;

            default:
                userStack = stacksList.FirstOrDefault(s => s.Name == UserInterface.OptionChoice);
                if (userStack == null)
                {
                    throw new InvalidOperationException($"A stack with the name {UserInterface.OptionChoice}, does not exist in this context.");
                }
                else
                {
                    flashcards = FlashcardDb.GetByStackId(userStack.Id);
                    DisplayFlashcards(userStack);
                }
                break;
        }
    }

    private void HandleActionMenu()
    {
        switch (UserInterface.OptionChoice)
        {
            case "Go back":
                MenuManager.GoBack();
                break;
            case "Update a Flashcard":
                MenuManager.NewMenu(new UpdateFlashcardMenu(MenuManager, FlashcardDb, StackDb, SessionDb, flashcards, flashcardDtos, userStack));
                break;
            case "Delete a Flashcard":
                MenuManager.NewMenu(new DeleteFlashcardMenu(MenuManager, FlashcardDb, StackDb, SessionDb, flashcards, flashcardDtos, userStack));
                break;
            case "Delete a Stack":
                {
                    MenuManager.NewMenu(new DeleteStackMenu(MenuManager, FlashcardDb, StackDb, SessionDb, stacksList));
                }
                break;
        }
    }
}

public class UpdateFlashcardMenu : ShowStacksMenu
{
    public UpdateFlashcardMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb, List<Flashcard> flashcards, List<FlashcardReviewDto> flashcardDtos, Stack userStack) : base(menuManager, flashcardDb, stackDb, sessionDb)
    {
        this.flashcards = flashcards;
        this.flashcardDtos = flashcardDtos;
        this.userStack = userStack;
    }

    public override void Display()
    {
        try
        {
            HandleFlashcardUpdate();
        }
        catch (KeyNotFoundException ex)
        {
            UserInput.DisplayMessage(ex.Message + " Update failed.", "go back", true);
        }

        MenuManager.GoBack();
    }
    private void HandleFlashcardUpdate()
    {
        while (true)
        {
            UserInterface.UpdateFlashcard(flashcardDtos, userStack);
            var userId = UserInput.FlashcardIdInput(MenuManager, flashcardDtos);

            UserInterface.UpdateFlashcardQuestion(userId);
            var userQuestion = UserInput.InputWithSpecialKeys(MenuManager, true, 50);

            UserInterface.UpdateFlashcardAnswer(userId, userQuestion);
            var userAnswer = UserInput.InputWithSpecialKeys(MenuManager, true, 50);

            UserInterface.UpdateFlashcardConfirm(userId, userQuestion, userAnswer, userStack.Name);
            if (UserInterface.OptionChoice == "Confirm")
            {
                var flashcardId = Operations.GetFlashcardDbId(userId, flashcards, flashcardDtos);
                if (flashcardId != -1)
                {
                    FlashcardDb.Update(userQuestion, userAnswer, userStack.Id, flashcardId);
                }
                else
                {
                    throw new KeyNotFoundException("The specified id was not found in the collection.");
                }
                break;
            }
        }
    }
}

public class DeleteFlashcardMenu : ShowStacksMenu
{
    public DeleteFlashcardMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb, List<Flashcard> flashcards, List<FlashcardReviewDto> flashcardDtos, Stack userStack) : base(menuManager, flashcardDb, stackDb, sessionDb)
    {
        this.flashcards = flashcards;
        this.flashcardDtos = flashcardDtos;
        this.userStack = userStack;
    }
    public override void Display()
    {
        try
        {
            HandleFlashcardDelete();
        }
        catch (KeyNotFoundException ex)
        {
            UserInput.DisplayMessage(ex.Message + " Deletion failed.", "go back", true);
        }

        MenuManager.GoBack();
    }
    private void HandleFlashcardDelete()
    {
        while (true)
        {
            UserInterface.DeleteFlashcard(flashcardDtos, userStack);
            var userId = UserInput.FlashcardIdInput(MenuManager, flashcardDtos);

            UserInterface.DeleteFlashcardConfirm(userId);
            if (UserInterface.OptionChoice == "Yes")
            {
                var flashcardId = Operations.GetFlashcardDbId(userId, flashcards, flashcardDtos);
                if (flashcardId != -1)
                {
                    FlashcardDb.Delete(flashcardId);
                }
                else
                {
                    throw new KeyNotFoundException("The specified id was not found in the collection.");
                }
                break;
            }
        }
    }
}

public class DeleteStackMenu : ShowStacksMenu
{
    public DeleteStackMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb, List<Stack> stacksList) : base(menuManager, flashcardDb, stackDb, sessionDb)
    {
        this.stacksList = stacksList;
    }

    public override void Display()
    {
        try
        {
            DisplayStacks();
            HandleUserOptions();
        }
        catch (KeyNotFoundException ex)
        {
            UserInput.DisplayMessage(ex.Message + " Deletion failed.", "go back", true);
        }

        MenuManager.GoBack();
    }

    private void DisplayStacks()
    {
        var stacksArray = Operations.StackListToNamesArray(stacksList);
        UserInterface.DeleteStack(stacksArray);
    }
    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case "Go back":
                MenuManager.GoBack();
                break;

            default:
                userStack = stacksList.FirstOrDefault(s => s.Name == UserInterface.OptionChoice);
                if (userStack == null)
                {
                    throw new InvalidOperationException($"A stack with the name {UserInterface.OptionChoice}, does not exist in this context.");
                }
                else
                {
                    HandleStackDelete(userStack);
                    MenuManager.DisplayCurrentMenu();
                }
                break;
        }
    }

    private void HandleStackDelete(Stack userStack)
    {
        UserInterface.DeleteStackConfirm(userStack);

        if (UserInterface.OptionChoice == "Yes")
        {
            StackDb.Delete(userStack.Id);
            UserInput.DisplayMessage($"Stack '{userStack.Name}' and it's flashcards have been deleted.", "go back", true);
        }
        else
            MenuManager.DisplayCurrentMenu();

    }
}
public class ShowStudySessionsMenu : Menu
{
    public ShowStudySessionsMenu(MenuManager menuManager, FlashcardDb flashcardDb, StackDb stackDb, StudySessionDb sessionDb) : base(menuManager, flashcardDb, stackDb, sessionDb) { }

    public override void Display()
    {
        UserInterface.ShowStudySessions();
    }
}