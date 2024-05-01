using Flashcards.DAO;
using Flashcards.Enums;
using Flashcards.Services;
using Flashcards.Database;
using Spectre.Console;
using Flashcards.DTO;
using Flashcards.Application.Helpers;

namespace Flashcards.Application;

public class AppManageStacks
{
    private readonly StackDao _stackDao;
    private readonly FlashCardDao _flashCardDao;
    private readonly InputHandler _inputHandler;
    private readonly ManageStacksHelper _manageStacksHelper;
    private readonly string _pageHeader = "Manage Stacks";
    private bool _running;


    public AppManageStacks(DatabaseContext databaseContext, InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _stackDao = new StackDao(databaseContext);
        _flashCardDao = new FlashCardDao(databaseContext);
        _manageStacksHelper = new ManageStacksHelper(_stackDao, _flashCardDao, _inputHandler);

        _running = true;
    }

    public void Run()
    {
        while (_running)
        {
            AnsiConsole.Clear();
            Utilities.DisplayPageHeader(_pageHeader);
            PromptForSessionAction();
        }
    }

    private void PromptForSessionAction()
    {
        StackMenuOption selectedOption = _inputHandler.PromptMenuSelection<StackMenuOption>();
        ExecuteSelectedOption(selectedOption);
    }

    private void ExecuteSelectedOption(StackMenuOption option)
    {
        switch (option)
        {
            case StackMenuOption.ViewStacks:
                HandleViewStackSelection();
                break;
            case StackMenuOption.EditStack:
                HandleEditStackSelection();
                break;
            case StackMenuOption.DeleteStack:
                HandleDeleteStackSelection();
                break;
            case StackMenuOption.CreateStack:
                HandleCreateStackSelection();
                break;
            case StackMenuOption.Return:
                CloseSession();
                break;
        }
    }

    private void CloseSession()
    {
        _running = false;
    }

    private void HandleViewStackSelection()
    {
        AnsiConsole.Clear();
        IEnumerable<StackDto>? stacks = _manageStacksHelper.GetAllStacks();
        _manageStacksHelper.DisplayStacks(stacks);
    }

    private void HandleEditStackSelection()
    {
        AnsiConsole.Clear();
        IEnumerable<StackDto>? stacks = _manageStacksHelper.GetAllStacks();

        if (stacks == null)
        {
           _manageStacksHelper.HandleNoStacksFound();
            return;
        }

        StackDto stack = _inputHandler.PromptForSelectionListStacks(stacks, "Select a stack to edit:");

        _manageStacksHelper.HandleEditStack(stack);
    }

    private void HandleDeleteStackSelection()
    {
        AnsiConsole.Clear();
        IEnumerable<StackDto>? stacks = _manageStacksHelper.GetAllStacks();

        if (stacks == null)
        {
            _manageStacksHelper.HandleNoStacksFound();
            return;
        }

        StackDto stack = _inputHandler.PromptForSelectionListStacks(stacks, "Select a stack to edit:");
        
        if (_inputHandler.ConfirmAction($"Are you sure you want to delete the stack '{stack.StackName}'?"))
        {
            _manageStacksHelper.HandleDeleteStack(stack);
        }
    }

    private void HandleCreateStackSelection()
    {
        AnsiConsole.Clear();
        string stackName = _inputHandler.PromptForNonEmptyString("Enter the name of the stack:");
        StackDto stack = new StackDto { StackName = stackName };

        try
        {             
            _stackDao.CreateStack(stack);
            Utilities.DisplaySuccessMessage("Stack created successfully.");
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error creating stack.", ex.Message);
        }
    }
}
