using System.Configuration;

public class App
{
    private UserInput _userInput;
    private DatabaseManager _databaseManager;
    private IManageStacks _stackRepo;
    private IManageStacks _stackController;
    private List<Stack> stackList;

    public void Run()
    {
        _userInput = new UserInput();
        _databaseManager = new DatabaseManager(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        _stackRepo = new StackRepository(_databaseManager);
        _stackController = new StackController(_stackRepo);

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
            Console.WriteLine("No stacs to show. Press any key to continue to create a new stack.");
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
                break;
            case ManageStackOption.EditCard:
                break;
            case ManageStackOption.DeleteCard:
                break;
            case ManageStackOption.DeleteStack:
                break;
            case ManageStackOption.Exit:
                break;
        }
    }
}