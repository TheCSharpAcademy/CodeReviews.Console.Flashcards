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

    public StackDTO GetNewStackInput()
    {
        string newStackName = AnsiConsole.Ask<string>("Enter the name of the new stack: ");

        if (string.IsNullOrEmpty(newStackName))
        {
            newStackName = AnsiConsole.Ask<string>("The stack's name can't be empty. Enter a name: ");
        }

        return new StackDTO(newStackName);
    }
    public bool ConfirmStackDeletion(int questions)
    {
        return AnsiConsole.Confirm($"Are you sure you want to delete the stack? It has {questions} questions in it. All questions will be lost.", false);
    }

    public StackModel GetMatchingStackFromList(List<StackModel> stacks, string prompt)
    {
        string selection = AnsiConsole.Ask<string>(prompt);
        
        if (!stacks.Any(s => s.Name.Contains(selection, StringComparison.OrdinalIgnoreCase))
        )
        {
            selection = AnsiConsole.Ask<string>("That input is not valid. Make sure you spelled the name correctly");
            
        }

        StackModel matchingStack = stacks.FirstOrDefault(s => s.Name.Contains(selection, StringComparison.OrdinalIgnoreCase));

        return matchingStack;
    }

    public int GetCardIndex(string prompt)
    {
        int cardIndex = AnsiConsole.Ask<int>(prompt);

        return cardIndex;
    }
}
