using System.Configuration;

public class App
{
    private UserInput _userInput;
    private DatabaseManager _databaseManager;
    private IManageStacks _stackRepo;
    private IManageStacks _stackController;
    private IHandleFlashcards _flashcardRepo;
    private IHandleFlashcards _flashcardController;
    private List<Stack> stackList;

    public void Run()
    {
        _userInput = new UserInput();
        _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        _stackRepo = new StackRepository(_databaseManager);
        _stackController = new StackController(_stackRepo);
        _flashcardRepo = new FlashcardRepository(_databaseManager);
        _flashcardController = new FlashcardController(_flashcardRepo);

        _databaseManager.InitializeDB();

        while (true)
        {
            var mainMenuOption = _userInput.MainMenu();

            switch (mainMenuOption)
            {
                case MainMenuOptions.Stacks:
                    StackMenu();
                    break;
                case MainMenuOptions.Flashcards:
                    break;
                case MainMenuOptions.Study:
                    break;
                case MainMenuOptions.InsertTestData: // TODO after flashcard repo insert. Delete current table when we do this.
                    break;
                case MainMenuOptions.Exit:
                    Environment.Exit(0);
                    break;

            }
        }

    }

    public void StackMenu()
    {
        stackList = _stackController.GetStacks();

        if (stackList.Count == 0)
        {
            Console.WriteLine("No stacks to show. Press any key to continue to create a new stack.");
            Console.ReadKey(true);
        }
        else
        {
            Stack stack;
            var option = _userInput.StacksManu();
            switch (option)
            {
                case StackOptions.Insert:
                    stack = _userInput.InsertStack(stackList);
                    _stackController.CreateStack(stack.Name);
                    break;
                case StackOptions.Select:
                    stack = _userInput.SelectStack(stackList);
                    ManageStack(stack);
                    break;
                case StackOptions.Exit:
                    Console.Clear();
                    break;
            }
        }
    }

    public void ManageStack(Stack stack)
    {
        stackList = _stackController.GetStacks();

        var option = _userInput.ManageStacksManu(stack);

        switch (option)
        {
            case ManageStackOption.ChangeStack:
                stack = _userInput.SelectStack(stackList);
                ManageStack(stack);
                break;
            case ManageStackOption.ViewCardsAll:
                break;
            case ManageStackOption.ViewCardsAmount:
                break;
            case ManageStackOption.CreateCard:
                var flashcard = _userInput.CreateFlashcard(stack);
                _flashcardController.CreateFlashcard(flashcard);
                Console.WriteLine("Entry added");
                break;
            case ManageStackOption.EditCard:
                break;
            case ManageStackOption.DeleteCard:
                break;
            case ManageStackOption.DeleteStack:
                _stackController.DeleteStack(stack.Name);
                break;
            case ManageStackOption.Exit:
                Console.Clear();
                break;
        }
    }
}