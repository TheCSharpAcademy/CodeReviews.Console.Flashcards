using System;
using Flash_Cards.Lawang.Controller;
using Flash_Cards.Lawang.Models;
using Flash_Cards.Lawang.Views;

namespace Flash_Cards.Lawang;

public class ManageStacks
{
    private Validation _validation;
    private Visualize _visual;
    private StackController _stackController;

    public ManageStacks(Validation validation, Visualize visual, StackController stackController)
    {
        _validation = validation;
        _visual = visual;
        _stackController = stackController;
    }
    public void OperationMenu()
    {
        List<Option> listOfOption = new List<Option>()
        {
            new Option("Create Stack.", 1),
            new Option("Update Stack.", 2),
            new Option("Delete Stack.", 3),
            new Option("Show Stack.", 4),
            new Option("Exit", 5)
        };
        bool exitOption = false;
        do
        {
            var option = _validation.ChooseOption(listOfOption, "STACK OPTIONS", "[bold cyan underline]What [green]opertion[/] do you want to perform?[/]");
            switch (option.Value)
            {
                case 1:
                    CreateStack();
                    break;
                case 2:
                    UpdateStack();
                    break;
                case 3:
                    DeleteStack();
                    break;
                case 4:
                    List<Stack> stackList = _stackController.GetAllStacks();
                    _visual.RenderStackTable(stackList);
                    if (stackList.Count != 0)
                    {
                        Console.ReadLine();
                    }
                    break;
                case 5:
                    exitOption = true;
                    break;
            }
            Console.Clear();
        } while (!exitOption);

    }

    private void CreateStack()
    {
        var stackList = _stackController.GetAllStacks();
        string? userInput = _validation.ValidateStackName(stackList);
        if (userInput == null)
        {
            return;
        }
        

        int rowsAffected = _stackController.CreateStack(userInput);

        _visual.RenderResult(rowsAffected);
    }

    private void UpdateStack()
    {
        List<Stack> stackList = _stackController.GetAllStacks();
        _visual.RenderStackTable(stackList);

        Stack? stack = _validation.UpdateStack(stackList);

        if (stack == null)
        {
            return;
        }

        Stack? updateStack = _validation.UpdateStackName(stack, stackList);
        if (updateStack == null)
        {
            return;
        }
        int affectedRow = _stackController.UpdateStack(updateStack);

        _visual.RenderResult(affectedRow);
    }

    private void DeleteStack()
    {
        List<Stack> stackList = _stackController.GetAllStacks();
        _visual.RenderStackTable(stackList);

        int stackId = _validation.DeleteStack(stackList);

        if (stackId == 0)
        {
            return;
        }

        int rowAffected = _stackController.DeleteStack(stackId);

        _visual.RenderResult(rowAffected);
    }
}
