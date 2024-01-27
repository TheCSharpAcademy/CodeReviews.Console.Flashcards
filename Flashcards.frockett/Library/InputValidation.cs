using Spectre.Console;
using Library.Models;

namespace Library;

public class InputValidation
{
    public CardModel GetNewFlashCardInput()
    {
        throw new NotImplementedException();
    }

    public int GetStackId(List<StackModel> stacks)
    {
        string selection = AnsiConsole.Ask<string>("Which of the stacks would you like to study? Type the name as listed above.\n");
        int stackIdToSelect = 0;
        
        if (!stacks.Any(s => s.Name.Contains(selection, StringComparison.OrdinalIgnoreCase))
        )
        {
            selection = AnsiConsole.Ask<string>("That input is not valid. Make sure you spelled the name correctly");
            
        }

        StackModel matchingStack = stacks.FirstOrDefault(s => s.Name.Contains(selection, StringComparison.OrdinalIgnoreCase));

        return stackIdToSelect = matchingStack.Id;
    }
}
