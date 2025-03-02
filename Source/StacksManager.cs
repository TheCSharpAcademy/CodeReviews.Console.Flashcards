using Dapper;
using Spectre.Console;

namespace vcesario.Flashcards;

public class StacksManager
{
    enum MenuOption
    {
        AddCards,
        EditCard,
        DeleteCard,
        DeleteStack,
        AddDebugCards,
        Return,
    }
    
    public void Open()
    {
        Console.Clear();
        Console.WriteLine(ApplicationTexts.STACKSMANAGER_HEADER);

        List<StackObject> stacks;
        using (var connection = DataService.OpenConnection())
        {
            string sql = "SELECT Id, Name FROM Stacks";
            stacks = connection.Query<StackObject>(sql).ToList();
        }

        var prompt = new SelectionPrompt<StackObject>()
                    .Title(ApplicationTexts.STACKSMANAGER_PROMPT_SELECTSTACK)
                    .AddChoices(stacks)
                    .UseConverter(stackObject => stackObject.Name);
        prompt.AddChoice(new StackObject(-1, ApplicationTexts.OPTION_RETURN));

        Console.WriteLine();
        var chosenStack = AnsiConsole.Prompt(prompt);
        if (chosenStack.Id == -1)
        {
            return;
        }

        bool choseReturn = false;
        do
        {
            Console.Clear();
            AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STACKSMANAGER_HEADER_SINGLE, $"[cornflowerblue]{chosenStack.Name}[/]"));

            Console.WriteLine();
            var chosenOption = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                .Title(ApplicationTexts.PROMPT_ACTION)
                .AddChoices(Enum.GetValues<MenuOption>()));

            switch (chosenOption)
            {
                case MenuOption.DeleteStack:
                    if (AskDeleteStack(chosenStack.Id))
                    {
                        goto case MenuOption.Return;
                    }
                    break;
                case MenuOption.AddDebugCards:
                    AddDebugCards(chosenStack.Id);
                    break;
                case MenuOption.Return:
                    choseReturn = true;
                    break;
                default:
                    break;
            }
        }
        while (!choseReturn);
    }

    private bool AskDeleteStack(int stackId)
    {
        var answer = AnsiConsole.Prompt(
            new ConfirmationPrompt(ApplicationTexts.STACKSMANAGER_PROMPT_DELETESTACK)
            {
                DefaultValue = false
            }
        );

        if (!answer)
        {
            return false;
        }

        answer = AnsiConsole.Prompt(
            new ConfirmationPrompt(ApplicationTexts.PROMPT_REALLYDELETE)
            {
                DefaultValue = false
            }
        );

        if (!answer)
        {
            return false;
        }

        using (var connection = DataService.OpenConnection())
        {
            string sql = "DELETE FROM Stacks WHERE Id = @StackId";
            try
            {
                connection.Execute(sql, new { StackId = stackId });
                Console.WriteLine(ApplicationTexts.STACKSMANAGER_LOG_STACKDELETED);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
            }
        }

        return true;
    }

    private void AddDebugCards(int stackId)
    {
        List<CardDTO> debugCards = new(){
            new CardDTO("A","1"),
            new CardDTO("B","2"),
            new CardDTO("C","3"),
            new CardDTO("D","4"),
            new CardDTO("E","5"),
            new CardDTO("F","6"),
            new CardDTO("G","7"),
            new CardDTO("H","8"),
            new CardDTO("I","9"),
            new CardDTO("J","10"),
        };

        using (var connection = DataService.OpenConnection())
        {
            try
            {
                foreach (var card in debugCards)
                {
                    string sql = "INSERT INTO Cards(StackId, Front, Back) VALUES (@StackId, @Front, @Back)";
                    connection.Execute(sql, new { StackId = stackId, Front = card.Front, Back = card.Back });
                }

                Console.WriteLine(ApplicationTexts.STACKSMANAGER_LOG_DEBUGCREATED);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }
    }
}