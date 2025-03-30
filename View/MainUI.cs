using Spectre.Console;

class MainUI
{
    public static Enums.MainMenuOptions MainMenu()
    {
        const string ManageStacks = "Manage stacks";
        const string ManageFlashCards = "Manage flash cards";
        const string Study = "Study";
        const string Exit = "Exit";

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

    public static Enums.ManageStacksMenuOptions ManageStacksMenu()
    {
        const string CreateStack = "Create new stack";
        const string RenameStack = "Rename stack";
        const string DeleteStack = "Delete stack";
        const string Exit = "Exit";
        string userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold green] Select option[/]")
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .AddChoices([
                    CreateStack, RenameStack, DeleteStack,
                    Exit,
                ])
        );

        switch (userInput)
        {
            case CreateStack:
                return Enums.ManageStacksMenuOptions.CREATESTACK;
            case RenameStack:
                return Enums.ManageStacksMenuOptions.RENAMESTACK;
            case DeleteStack:
                return Enums.ManageStacksMenuOptions.DELETESTACK;
            case Exit:
                return Enums.ManageStacksMenuOptions.EXIT;
        }
        return default;
    }
}