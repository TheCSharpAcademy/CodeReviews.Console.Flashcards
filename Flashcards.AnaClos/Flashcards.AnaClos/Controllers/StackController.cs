using Flashcards.AnaClos.DTOs;
using Flashcards.AnaClos.Models;
using Spectre.Console;

namespace Flashcards.AnaClos.Controllers;

public class StackController
{
    private ConsoleController _consoleController;
    private DataBaseController _dataBaseController;

    public StackController(ConsoleController consoleController, DataBaseController dataBaseController)
    {
        _consoleController = consoleController;
        _dataBaseController = dataBaseController;
    }

    public Stack? AddStack()
    {
        string message = "Please enter a stack name or enter 0 to return to main menu:";
        string response = _consoleController.GetString(message);

        if (response.Trim() == "0") return null;

        var stack = new Stack{Name=response};
        var sqlInsert = "INSERT INTO Stacks (Name) VALUES (@Name)"; 
        try
        {
            int rows = _dataBaseController.Execute<Stack>(sqlInsert, stack);
            stack = GetStackByName(response);
            _consoleController.ShowMessage($"{rows} stack added.","green");
            _consoleController.PressKey("Press any key to continue");
        }
        catch(Exception ex)
        {
            stack = null;

            _consoleController.ShowMessage(ex.Message,"red bold");
            _consoleController.PressKey("Press any key to continue");
        }
        return stack;
    }

    public Stack? GetStackByName(string name)
    {
        Stack? stack;
        var sql = $"SELECT * FROM Stacks WHERE Name = '{name}'";
        try
        {
            stack = _dataBaseController.QuerySingle<Stack>(sql);
        }
        catch (Exception ex)
        {
            stack = null;
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
        return stack;
    }

    public Stack? GetStackById(int id)
    {
        Stack? stack;
        var sql = @"SELECT * FROM Stacks WHERE Id = '{id}'";
        try
        {
            stack = _dataBaseController.QuerySingle<Stack>(sql);
        }
        catch (Exception ex)
        {
            stack = null;
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
        return stack;
    }


    public List<Stack> GetStacks()
    {
        var stacks = new List<Stack>();
        var sql = "SELECT * FROM Stacks";
        try
        {
            stacks = _dataBaseController.Query<Stack>(sql);
        }
        catch (Exception ex)
        {
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
        return stacks;
    }

    public List<string> GetStackNames(List<Stack> stacks)
    {
        var stackNames = stacks.Select(x => x.Name).ToList();
        return stackNames;
    }
    //When a stack is deleted, the associated flashcards and study sessions for that stack are also deleted from the database.
    public void DeleteStack()
    {
        string title = "Select a stack to delete.";
        string returnOption = "Return to Main";
        var stacks = GetStacks();
        var stackNames = GetStackNames(stacks);
        
        stackNames.Add(returnOption);
        string response = _consoleController.Menu(title,"blue",stackNames);
        if (response == returnOption)
        {
            return;
        }
        var stack = new Stack { Name = response };
        var sql = "DELETE Stacks WHERE Name = @Name";
        int rows;
        try
        {
            rows = _dataBaseController.Execute<Stack>(sql, stack);
            _consoleController.ShowMessage($"{rows} stack deleted.", "green");
            _consoleController.PressKey("Press any key to continue");
        }
        catch (Exception ex)
        {
            _consoleController.ShowMessage(ex.Message, "red bold");
            _consoleController.PressKey("Press any key to continue");
        }
    }
}
