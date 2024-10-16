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

    public Stack AddStack()
    {
        string message = "Please enter a stack name or enter # to return to main menu:";
        string response = _consoleController.GetString(message);

        if (response.Trim() == "#") return null;

        var stack = new Stack{Name=response};
        var sqlInsert = "INSERT INTO Stacks (Name) VALUES (@Name)"; 
        try
        {
            int rows = _dataBaseController.Execute<Stack>(sqlInsert, stack);
            stack = GetStackByName(response);
            _consoleController.MessageAndPressKey($"{rows} stack added.", "green");
        }
        catch(Exception ex)
        {
            stack = null;
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
        return stack;
    }

    public Stack GetStackByName(string name)
    {
        Stack stack;
        var sql = $"SELECT * FROM Stacks WHERE Name = '{name}'";
        try
        {
            stack = _dataBaseController.QuerySingle<Stack>(sql);
        }
        catch (Exception ex)
        {
            stack = null;
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
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
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
        return stacks;
    }

    public List<string> GetStackNames(List<Stack> stacks)
    {
        var stackNames = stacks.Select(x => x.Name).ToList();
        return stackNames;
    }
    
    public string ShowStacks(string title, string returnOption)
    {
        var stacks = GetStacks();
        List<string> stackNames = new();

        if (stacks.Count > 0)
        {
            stackNames = GetStackNames(stacks);
        }
        else
        {
            title = "There are no Stacks";
        }

        stackNames.Add(returnOption);

        return _consoleController.Menu(title, "blue", stackNames);
    }    

    public void DeleteStack()
    {
        string returnOption = "Return to Main";
        string title = "Select a stack to delete.";
        string response =ShowStacks(title,returnOption);

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
            _consoleController.MessageAndPressKey($"{rows} stack deleted.", "green");
        }
        catch (Exception ex)
        {
            _consoleController.MessageAndPressKey(ex.Message, "red bold");
        }
    }
}