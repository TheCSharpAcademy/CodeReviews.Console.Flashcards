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
}
