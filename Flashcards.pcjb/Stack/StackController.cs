namespace Flashcards;

class StackController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;

    public StackController(Database database)
    {
        this.database = database;
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    public void ShowList()
    {
        ShowList(null);
    }

    public void ShowList(string? message)
    {
        var stacks = database.ReadAllStacks();
        List<StackDto> stackDtos = new();
        foreach (Stack stack in stacks)
        {
            stackDtos.Add(new StackDto(stack.Name));
        }
        var view = new StackListView(this, stackDtos);
        view.SetMessage(message);
        view.Show();
    }

    public void SelectStack(string name)
    {
        var selectedStack = database.ReadStackByName(name);
        AppState.CurrentWorkingStack = selectedStack;
        if (selectedStack == null)
        {
            ShowList($"No stack found with name '{name}'.");
        }
        else
        {
            switch (AppState.CurrentMode)
            {
                case AppState.Mode.ManageStacks:
                    BackToMainMenu($"Selected stack: '{selectedStack.Name}'");
                    break;
                case AppState.Mode.ManageFlashcards:
                    ManageFlashcards();
                    break;
                default:
                    BackToMainMenu($"Selected stack: '{selectedStack.Name}'");
                    break;
            }

        }
    }

    public void ManageFlashcards()
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ManageFlashcards();
    }

    public void BackToMainMenu()
    {
        BackToMainMenu(null);
    }

    public void BackToMainMenu(string? message)
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ShowMainMenu(message);
    }
}