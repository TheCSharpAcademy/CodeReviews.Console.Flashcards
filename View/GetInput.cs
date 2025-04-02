using Spectre.Console;
class GetInput
{
    public static Stack Selection(List<Stack> dataSet)
    {
        var prompt = new SelectionPrompt<Stack>()
            .Title("[bold green]Select[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more data)[/]");
        
        foreach (var data in dataSet)
        {
            prompt.AddChoice(data);
        }

        prompt.Converter = data => $"{data.Id} {data.Name}";

        return AnsiConsole.Prompt(prompt);
    }

    public static FlashcardDTO Selection(List<FlashcardDTO> dataSet)
    {
        var prompt = new SelectionPrompt<FlashcardDTO>()
            .Title("[bold green]Select[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more data)[/]");
        
        foreach (var data in dataSet)
        {
            prompt.AddChoice(data);
        }

        prompt.Converter = data => $"{data.Id} {data.Front} {data.Back}";

        return AnsiConsole.Prompt(prompt);
    }
    public static string StackName()
    {
        return AnsiConsole.Prompt(new TextPrompt<string>("[bold green]New name for Stack: [/]"));
    }

    public static void FlashcardSides(out string front, out string back, 
                                    string currentFront = "", string currentBack = "")
    {
        TextPrompt<string> frontPrompt = new TextPrompt<string>("[bold green]Front of card: [/]");
        TextPrompt<string> backPrompt = new TextPrompt<string>("[bold green]Back of card: [/]");

        if (currentFront != "")
            frontPrompt.DefaultValue(currentFront);
        if (currentBack != "")
            backPrompt.DefaultValue(currentBack);

        front = AnsiConsole.Prompt(frontPrompt);
        back = AnsiConsole.Prompt(backPrompt);
    }

    public static int AmountOfCards()
    {
        var number = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter amount of cards")
            .Validate((n) =>
            {
                if (n < 0) return ValidationResult.Error("Cannot be under 0");
                return ValidationResult.Success();
            }));
        return number;
    }
}