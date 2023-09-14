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

    public void ShowMenu()
    {
        ShowMenu(null);
    }

    public void ShowMenu(string? message)
    {
        var view = new StackMenuView(this);
        view.SetMessage(message);
        view.Show();
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
                    ShowMenu($"Selected stack: '{selectedStack.Name}'");
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

    public void ShowCreate()
    {
        ShowCreate(null);
    }

    public void ShowCreate(string? message)
    {
        var view = new StackCreateView(this);
        view.SetMessage(message);
        view.Show();
    }

    public void Create(string? name)
    {
        if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
        {
            ShowMenu();
            return;
        }

        var cleanName = name.Trim();
        if (database.CreateStack(cleanName))
        {
            ShowMenu($"OK - Stack created '{cleanName}'");
        }
        else
        {
            ShowMenu("ERROR - Failed to save new stack.");
        }
    }

    public void ShowEdit()
    {
        ShowEdit(null);
    }

    public void ShowEdit(string? message)
    {
        if (AppState.CurrentWorkingStack == null)
        {
            ShowList();
        }
        else
        {
            var stack = database.ReadStackById(AppState.CurrentWorkingStack.Id);
            if (stack == null)
            {
                ShowMenu("ERROR - Failed to read stack from database.");
            }
            else
            {
                var view = new StackEditView(this, stack);
                view.SetMessage(message);
                view.Show();
            }
        }
    }

    public void Update(long stackId, string? newName)
    {
        if (String.IsNullOrEmpty(newName) || String.IsNullOrWhiteSpace(newName))
        {
            ShowMenu();
            return;
        }

        var cleanName = newName.Trim();
        if (database.UpdateStack(stackId, cleanName))
        {
            ShowMenu($"OK - Stack updated '{cleanName}'");
        }
        else
        {
            ShowMenu("ERROR - Failed to update stack.");
        }
    }

    public void ShowDelete()
    {
        if (AppState.CurrentWorkingStack == null)
        {
            ShowList();
            return;
        }
        var stack = database.ReadStackById(AppState.CurrentWorkingStack.Id);
        if (stack == null)
        {
            ShowMenu("ERROR - Failed to read stack from database.");
            return;
        }
        var view = new StackDeleteView(this, stack);
        view.Show();
    }

    public void Delete(Stack stack)
    {
        if (database.DeleteStack(stack.Id))
        {
            ShowMenu($"OK - Stack deleted '{stack.Name}'");
            return;
        }
        ShowMenu($"ERROR - Failed to delete stack '{stack.Name}'");
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