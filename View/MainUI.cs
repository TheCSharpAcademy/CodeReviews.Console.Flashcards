using Spectre.Console;

class MainUI
{
    const string ManageStacks = "Manage stacks";
    const string ManageFlashCards = "Manage flash cards";
    const string Study = "Study";
    const string Exit = "Exit";
    public static Enums.MainMenuOptions MainMenu()
    {
        string userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green] Select option[/]")
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .AddChoices([
                    ManageStacks, ManageFlashCards, Study,
                    Exit,
                ])
        );

        switch (userInput)
        {
            case ManageStacks:
                return Enums.MainMenuOptions.MANAGESTACKS;
            case ManageFlashCards:
                return Enums.MainMenuOptions.MANAGEFLASHCARDS;
            case Study:
                return Enums.MainMenuOptions.STUDY;
            case Exit:
                return Enums.MainMenuOptions.EXIT;
        }
        return default;
    }

    public static Enums.ManageStacksOptions ManageStacksMenu()
    {
        return default;
    }
}