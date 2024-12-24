using FlashCards.Controllers;
using FlashCards.Data;
using FlashCards.Models;
using FlashCards.View;
using Spectre.Console;
using static FlashCards.Enums;

namespace FlashCards.Services;

internal class StackService : ConsoleController
{
    private readonly DataContext _context;

    public StackService(DataContext context)
    {
        _context = context;
    }

    public Stack? SelectStackToManage()
    {
        var stacks = GetAll();

        if (stacks.Count == 0)
        {
            ErrorMessage("No stacks available.");
            return null;
        }

        var stackChoices = stacks.Select(s => s.Name).ToList();
        var selectedChoice = UserInterface.ManageAllItemsMenu(stackChoices);

        Stack stack = stacks.FirstOrDefault(s => s.Name == selectedChoice);

        if (stack == null)
        {
            ErrorMessage("Stack was not found");
        }

        return stack;

    }

    public static void ShowFull(Stack stack) 
    {
        try
        {
            Table table = DrawStackTable();
            table.AddRow(stack.Id.ToString(), stack.Name, stack.StudySessions.Count.ToString(), stack.Flashcards.Count.ToString());
            AnsiConsole.Write(table);

            UserInterface.StandardMenu();
        } catch (Exception ex)
        {
            ErrorMessage(ex.Message);
        }
    }

    public void Add(Stack stack)
    {
        try
        {
            if (CheckIfExistsByName(stack.Name))
            {
                ErrorMessage("Dear user, a stack with such a name already exists. The name must be unique.");
                return;
            }

            _context.Stacks.Add(stack);
            _context.SaveChanges();
            SuccessMessage($"The stack with a name of {stack.Name} has been added successfully.");
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error while adding the stack: {ex.Message}");
        }
    }

    public void Edit(ManageStackEditOptions editChoice, Stack stack) 
    {
        try
        {
            if (CheckIfExistsByName(stack.Name))
            {
                ErrorMessage("Dear user, a stack with such a name already exists. The name must be unique.");
                return;
            }

            Stack savedStack = GetById(stack.Id) ?? throw new Exception("Stack was not found, or the database was not connected properly.");

            //Implementing such a solution in case of adding more columns in the db.
            switch (editChoice)
            {
                case ManageStackEditOptions.Name:
                    savedStack.Name = stack.Name;
                    break;
                default:
                    break;
            }

            _context.SaveChanges();
            SuccessMessage($"The stack with an id of {stack.Id} has been updated successfully.");
        }
        catch (Exception ex) 
        {
            ErrorMessage($"There has been an error while editing the stack: {ex.Message}");
        }
    }

    public void Delete(int id) {
        try
        {
            Stack stack = GetById(id);
            _context.Stacks.Remove(stack);
            _context.SaveChanges();

            SuccessMessage($"The stack with an id of {id} has been deleted successfully.");
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error while deleting the stack: {ex.Message}");
            return;
        }
    }

    public bool CheckIfExistsByName(string name)
    {
        var stack = _context.Stacks.FirstOrDefault(s => s.Name == name);
        if (stack == null)
            return false;
        return true;
    }

    public Stack GetById(int id)
    {
        return _context.Stacks.FirstOrDefault(s => s.Id == id);
    }

    public List<Stack>? GetAll()
    {
        return _context.Stacks.ToList();
    }
}
