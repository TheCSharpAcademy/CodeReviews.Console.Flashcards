using Dapper;
using Microsoft.Identity.Client;
using Spectre.Console;

namespace vcesario.Flashcards;

public class StacksManager
{
    enum MenuOption
    {
        AddCards,
        DeleteCards,
        RenameStack,
        DeleteStack,
        AddDebugCards,
        Return,
    }

    public void Open()
    {
        Console.Clear();
        Console.WriteLine(ApplicationTexts.STACKSMANAGER_HEADER);

        var chosenStack = PromptStack();
        if (chosenStack.Id == -1)
        {
            return;
        }

        MenuOption chosenOption;
        do
        {
            Console.Clear();
            AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STACKSMANAGER_HEADER_SINGLE, $"[cornflowerblue]{chosenStack.Name}[/]"));

            Console.WriteLine();
            PrintCardsTable(chosenStack);

            Console.WriteLine();
            chosenOption = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                .Title(ApplicationTexts.PROMPT_ACTION)
                .AddChoices(Enum.GetValues<MenuOption>()));

            switch (chosenOption)
            {
                case MenuOption.AddCards:
                    PromptAddCards(chosenStack);
                    break;
                case MenuOption.DeleteCards:
                    PromptDeleteCards(chosenStack);
                    break;
                case MenuOption.RenameStack:
                    PromptRenameStack(chosenStack);
                    break;
                case MenuOption.DeleteStack:
                    if (PromptDeleteStack(chosenStack.Id))
                    {
                        chosenOption = MenuOption.Return;
                    }
                    break;
                case MenuOption.AddDebugCards:
                    AddDebugCards(chosenStack.Id);
                    break;
                case MenuOption.Return:
                default:
                    break;
            }
        }
        while (chosenOption != MenuOption.Return);
    }

    public StackObject PromptStack()
    {
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
        return chosenStack;
    }

    private void PromptAddCards(StackObject stack)
    {
        do
        {
            Console.Clear();
            AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STACKSMANAGER_HEADER_ADDCARD, $"[cornflowerblue]{stack.Name}[/]"));

            Console.WriteLine();
            AnsiConsole.MarkupLine($"[grey]{ApplicationTexts.TOOLTIP_LEAVE}[/]");

            Console.WriteLine();
            var cardFront = AnsiConsole.Prompt(
                new TextPrompt<string>(ApplicationTexts.STACKSMANAGER_PROMPT_ADDCARD_FRONT));

            if (cardFront.Equals("."))
            {
                return;
            }

            var cardBack = AnsiConsole.Prompt(
                new TextPrompt<string>(ApplicationTexts.STACKSMANAGER_PROMPT_ADDCARD_BACK));

            if (cardBack.Equals("."))
            {
                return;
            }

            using (var connection = DataService.OpenConnection())
            {
                try
                {
                    var sql = "INSERT INTO Cards(StackId, Front, Back) VALUES (@StackId, @Front, @Back)";
                    connection.Execute(sql, new { StackId = stack.Id, Front = cardFront, Back = cardBack });

                    Console.WriteLine();
                    AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STACKSMANAGER_LOG_CARDADDED, $"[gold3_1]{cardFront}[/] => [indianred]{cardBack}[/]"));
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ReadLine();
                }
            }
        }
        while (true);
    }

    private void PromptDeleteCards(StackObject stack)
    {
        List<CardDTO_IdFrontBack>? cards = null;
        using (var connection = DataService.OpenConnection())
        {
            try
            {
                string sql = "SELECT Id, Front, Back FROM Cards WHERE StackId = @StackId";
                cards = connection.Query<CardDTO_IdFrontBack>(sql, new { StackId = stack.Id }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
            }
        }

        do
        {
            Console.Clear();
            AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STACKSMANAGER_HEADER_DELETECARD, $"[cornflowerblue]{stack.Name}[/]"));

            if (cards == null || cards.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine(ApplicationTexts.STACKSMANAGER_LOG_STACKEMPTY);
                Console.ReadLine();
                return;
            }

            var prompt = new SelectionPrompt<CardDTO_IdFrontBack>()
                        .Title(ApplicationTexts.STACKSMANAGER_PROMPT_DELETECARD)
                        .AddChoices(cards)
                        .UseConverter(card => $"{card.Front} ({card.Back})");
            prompt.AddChoice(new CardDTO_IdFrontBack(-1, ApplicationTexts.OPTION_RETURN, ApplicationTexts.OPTION_RETURN));

            Console.WriteLine();
            var chosenCard = AnsiConsole.Prompt(prompt);
            if (chosenCard.Id == -1)
            {
                return;
            }

            cards.Remove(chosenCard);
            using (var connection = DataService.OpenConnection())
            {
                try
                {
                    string sql = "DELETE FROM Cards WHERE Id=@Id";
                    connection.Execute(sql, new { Id = chosenCard.Id });
                    Console.WriteLine(ApplicationTexts.STACKSMANAGER_LOG_CARDDELETED);
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ReadLine();
                }
            }
        }
        while (true);
    }

    private void PromptRenameStack(StackObject stack)
    {
        Console.Clear();
        AnsiConsole.MarkupLine(string.Format(ApplicationTexts.STACKSMANAGER_HEADER_ADDCARD, $"[cornflowerblue]{stack.Name}[/]"));

        Console.WriteLine();
        AnsiConsole.MarkupLine($"[grey]{ApplicationTexts.TOOLTIP_LEAVE}[/]");

        Console.WriteLine();
        var newName = AnsiConsole.Prompt(
            new TextPrompt<string>(ApplicationTexts.STACKSMANAGER_PROMPT_RENAMESTACK)
        );

        if (newName.Equals("."))
        {
            return;
        }

        using (var connection = DataService.OpenConnection())
        {
            try
            {
                var sql = @"UPDATE Stacks SET Name=@Name WHERE Id=@Id";
                connection.Execute(sql, new { Name = newName, Id = stack.Id });

                Console.WriteLine();
                Console.WriteLine(ApplicationTexts.STACKSMANAGER_LOG_STACKUPDATED);
                Console.ReadLine();

                stack.UpdateName(newName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
            }
        }
    }

    private bool PromptDeleteStack(int stackId)
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
        List<CardDTO_FrontBack> debugCards = new(){
            new CardDTO_FrontBack("A","1"),
            new CardDTO_FrontBack("B","2"),
            new CardDTO_FrontBack("C","3"),
            new CardDTO_FrontBack("D","4"),
            new CardDTO_FrontBack("E","5"),
            new CardDTO_FrontBack("F","6"),
            new CardDTO_FrontBack("G","7"),
            new CardDTO_FrontBack("H","8"),
            new CardDTO_FrontBack("I","9"),
            new CardDTO_FrontBack("J","10"),
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

    public List<CardDTO_FrontBack>? GetCards_FrontBack(StackObject stack)
    {
        List<CardDTO_FrontBack>? cards = null;
        using (var connection = DataService.OpenConnection())
        {
            try
            {
                string sql = "SELECT Front, Back FROM Cards WHERE StackId = @StackId";
                cards = connection.Query<CardDTO_FrontBack>(sql, new { StackId = stack.Id }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
            }
        }
        return cards;
    }
    private void PrintCardsTable(StackObject stack)
    {
        var cards = GetCards_FrontBack(stack);
        var table = new Table();

        table.AddColumn(new TableColumn($"[yellow]{ApplicationTexts.TABLE_ID}[/]").RightAligned());
        table.AddColumn(new TableColumn($"[yellow]{ApplicationTexts.TABLE_CARDFRONT}[/]").Centered());
        table.AddColumn(new TableColumn($"[yellow]{ApplicationTexts.TABLE_CARDBACK}[/]").Centered());

        if (cards == null || cards.Count == 0)
        {
            table.AddRow("---", "----------", "----------");
        }
        else
        {
            int count = 1;
            foreach (var card in cards)
            {
                table.AddRow(count++.ToString(), card.Front, card.Back);
            }
        }

        table.Border = TableBorder.Rounded;

        AnsiConsole.Write(table);
    }
}