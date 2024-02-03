using DataAccess;
using Library.Models;

namespace Library;

public class StackController
{
    private readonly IDataAccess dataAccess;
    private readonly InputValidation inputValidation;

    public StackController(IDataAccess dataAccess, InputValidation inputValidation)
    {
        this.dataAccess = dataAccess;
        this.inputValidation = inputValidation;
    }

    public List<StackDto> GetListOfStacks()
    {
        var listOfAllStacks = dataAccess.GetListOfStacks();
        List<StackDto> stackDTOs = listOfAllStacks.Select(stack => new StackDto(stack.Name)).ToList();
        return stackDTOs;
    }

    public StackDto GetStackDtoByName()
    {
        StackModel stack = inputValidation.GetMatchingStackFromList(dataAccess.GetListOfStacks(), "Enter the name of the stack you wish to select: ");

        StackDto stackDTO = new StackDto(stack.Name);
        stackDTO.StackId = stack.Id;
        return stackDTO;
    }
    public StackModel GetStackById(int stackId)
    {
        StackModel currentStack = dataAccess.GetStackById(stackId);
        return currentStack;
    }

    public void InsertStack()
    {
        StackDto stackDTO = inputValidation.GetNewStackInput();
        dataAccess.InsertStack(ToStackDomainModel(stackDTO));
    }

    public void DeleteStackById()
    {
        StackModel stackToDelete = inputValidation.GetMatchingStackFromList(dataAccess.GetListOfStacks(), "Enter the name of the stack you want to delete: ");
        int stackIdToDelete = stackToDelete.Id;
        int rowsInStack = dataAccess.CheckForStackContents(stackIdToDelete);
        bool deleteConfirmed = inputValidation.ConfirmStackDeletion(rowsInStack);

        if (deleteConfirmed)
        {
            dataAccess.DeleteStackById(stackIdToDelete);
        }
        else
        {
            return;
        }
    }

    private StackModel ToStackDomainModel(StackDto stackDTO)
    {
        StackModel stackModel = new StackModel();
        stackModel.Name = stackDTO.Name;
        return stackModel;
    }
}
