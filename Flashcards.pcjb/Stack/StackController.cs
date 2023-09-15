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

    public void ShowList(StackSelectionMode mode)
    {
        ShowList(mode, null);
    }

    public void ShowList(StackSelectionMode mode, string? message)
    {
        var stacks = database.ReadAllStacks();
        List<StackDto> stackDtos = new();
        foreach (Stack stack in stacks)
        {
            stackDtos.Add(new StackDto(stack.Name));
        }
        var view = new StackListView(this, stackDtos);
        view.SetMessage(message);
        view.SetStackSelectionMode(mode);
        view.Show();
    }

    public void SelectStack(string name, StackSelectionMode mode)
    {
        var selectedStack = database.ReadStackByName(name);
        if (selectedStack == null)
        {
            ShowList(mode, $"No stack found with name '{name}'.");
        }
        else
        {
            switch (mode)
            {
                case StackSelectionMode.None:
                    ShowMenu();
                    break;
                case StackSelectionMode.ForEdit:
                    ShowEdit(selectedStack);
                    break;
                case StackSelectionMode.ForDelete:
                    ShowDelete(selectedStack);
                    break;
                case StackSelectionMode.ForFlashcards:
                    ManageFlashcards(selectedStack);
                    break;
                case StackSelectionMode.ForStudySession:
                    Study(selectedStack);
                    break;
                default:
                    ShowMenu();
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
            ShowMenu($"OK - Created new stack '{cleanName}'");
        }
        else
        {
            ShowMenu("ERROR - Failed to save new stack.");
        }
    }

    public void ShowEdit(Stack stack)
    {
        ShowEdit(stack, null);
    }

    public void ShowEdit(Stack stack, string? message)
    {
        if (stack == null)
        {
            ShowMenu("ERROR - No stack selected for edit.");
        }
        else
        {
            var view = new StackEditView(this, stack);
            view.SetMessage(message);
            view.Show();
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

    public void ShowDelete(Stack stack)
    {
        if (stack == null)
        {
            ShowMenu("ERROR - No stack selected for delete.");
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

    public void ManageFlashcards(Stack selectedStack)
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ManageFlashcards(selectedStack);
    }

    public void Study(Stack selectedStack)
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.Study(selectedStack);
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