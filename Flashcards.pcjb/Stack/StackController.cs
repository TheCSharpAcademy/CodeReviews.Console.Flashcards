namespace Flashcards;

class StackController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;

    public StackController(Database database)
    {
        this.database = database;
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
        if (selectedStack == null)
        {
            ShowList($"No stack found with name '{name}'.");
        }
        else
        {
            BackToMainMenu($"Selected stack: '{selectedStack.Name}'");
        }
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
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