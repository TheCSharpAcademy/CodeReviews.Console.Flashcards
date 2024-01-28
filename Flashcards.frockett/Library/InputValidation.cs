using Spectre.Console;
using Library.Models;

namespace Library;

public class InputValidation
{
    public CardDTO GetNewFlashCardInput()
    {
        string question = AnsiConsole.Ask<string>("Enter the first side of the card: ");

        if (string.IsNullOrEmpty(question))
        {
            question = AnsiConsole.Ask<string>("The card can not be blank. Enter side one of the card: ");
        }

        string answer = AnsiConsole.Ask<string>("Enter the second side of the card: ");

        if (string.IsNullOrEmpty(answer))
        {
            question = AnsiConsole.Ask<string>("The card can not be blank. Enter side two of the card: ");
        }

        return new CardDTO(question, answer);
    }

    public int GetStackId(List<StackModel> stacks)
    {
        string selection = AnsiConsole.Ask<string>("Select the stack you're looking for. Type the name as listed above.\n");
        int stackIdToSelect = 0;
        
        if (!stacks.Any(s => s.Name.Contains(selection, StringComparison.OrdinalIgnoreCase))
        )
        {
            selection = AnsiConsole.Ask<string>("That input is not valid. Make sure you spelled the name correctly");
            
        }

        StackModel matchingStack = stacks.FirstOrDefault(s => s.Name.Contains(selection, StringComparison.OrdinalIgnoreCase));

        return stackIdToSelect = matchingStack.Id;
    }

    public int GetCardIndex(string prompt)
    {
        int cardIndex = AnsiConsole.Ask<int>(prompt);

        return cardIndex;
    }
}
