using System.Configuration;

public class App
{
    private UserInput _userInput;
    private DatabaseManager _databaseManager;
    private IManageStacks _stackRepo;
    private IManageStacks _stackController;

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
                    ManageStacks();
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

    public void ManageStacks() // TODO: Have a manage stacks class?
    {
        var stackList = _stackController.GetStacks();

        if (stackList.Count == 0)
        {
            Console.WriteLine("No stacs to show. Press any key to continue to create a new stack.");
            Console.ReadKey(true);
        }
        else
        {
            Stack stackOption;
            var option = _userInput.StacksManu();
            switch (option)
            {
                case StackOptions.Insert:
                    stackOption = _userInput.InsertStack(stackList);
                    _stackController.CreateStack(stackOption.Name);
                    break;
                case StackOptions.Select:
                    stackOption = _userInput.SelectStack(stackList);
                    break;
                case StackOptions.Exit:
                    Console.Clear();
                    break;
            }
        }

    }
}