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

        prompt.Converter = data => $"{data.Name}";

        return AnsiConsole.Prompt(prompt);
    }

    public static FlashcardDto Selection(List<FlashcardDto> dataSet)
    {
        var prompt = new SelectionPrompt<FlashcardDto>()
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
    
    public static string StackName(List<Stack> stacks)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>("[bold green]New name for Stack: [/]")
                .Validate(name => {
                    foreach (Stack stack in stacks)
                        if (stack.Name == name)
                            return ValidationResult.Error("[bold red]Must be unique name[/]");
                    return ValidationResult.Success();
                })
        );
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

    public static bool? StudyAnswer(string answer)
    {
        string userInput = AnsiConsole.Prompt(
            new TextPrompt<string>("Input your answer or press Enter to exit")
                .AllowEmpty()
        );

        if (userInput == "")
            return null;
        return userInput.Equals(answer);
    }
}