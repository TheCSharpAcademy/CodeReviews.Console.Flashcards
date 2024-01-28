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

    public List<StackDTO> GetListOfStacks()
    {
        var listOfAllStacks = dataAccess.GetListOfStacks();
        List<StackDTO> stackDTOs = listOfAllStacks.Select(stack => new StackDTO(stack.Name)).ToList();
        return stackDTOs;
    }

    public void InsertStack()
    {
        StackDTO stackDTO = inputValidation.GetNewStackInput();
        dataAccess.InsertStack(ToStackDomainModel(stackDTO));
    }

    public void DeleteStackById()
    {
        int stackToDelete = inputValidation.GetStackId(dataAccess.GetListOfStacks(), "Enter the name of the stack you want to delete: ");
        int rowsInStack = dataAccess.CheckForStackContents(stackToDelete);
        bool deleteConfirmed = inputValidation.ConfirmStackDeletion(rowsInStack);

        if (deleteConfirmed)
        {
            dataAccess.DeleteStackById(stackToDelete);
        }
        else
        {
            return;
        }
    }

    private StackDTO ToStackDTO(StackModel stack)
    {
        throw new NotImplementedException();
    }

    private StackModel ToStackDomainModel(StackDTO stackDTO)
    {
        StackModel stackModel = new StackModel();
        stackModel.Name = stackDTO.Name;
        return stackModel;
    }
}
