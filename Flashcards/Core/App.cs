using System.Security.Cryptography.X509Certificates;
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
        _stackRepo =  new StackRepository(_databaseManager);
        _stackController = new StackController(_stackRepo);

        _databaseManager.InitializeDB();

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

    public void ManageStacks() // TODO would it be better to have a ManageStacks class?
    {
        var stackList = _stackController.GetStacks();

        if (stackList.Count == 0)
        {
            // TODO call Create Stack here...
        }
        else
        {
            var stack = _userInput.SelectStack(stackList);
        }

    }
}