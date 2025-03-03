using Dapper;
using Spectre.Console;

namespace vcesario.Flashcards;

public static class MainApplication
{
    enum MainMenuOption
    {
        CreateNewStack,
        ManageStacks,
        StudyArea,
        Exit,
    }

    public static void Run()
    {
        DataService.Initialize();

        bool choseExit = false;
        do
        {
            Console.Clear();

            AnsiConsole.MarkupLine("[plum4]===== FLASHCARDS TRAINING =====[/]");
            Console.WriteLine();

            var chosenOption = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                .Title(ApplicationTexts.PROMPT_ACTION)
                .AddChoices(Enum.GetValues<MainMenuOption>())
            );

            switch (chosenOption)
            {
                case MainMenuOption.CreateNewStack:
                    CreateNewStack();
                    break;
                case MainMenuOption.ManageStacks:
                    new StacksManager().Open();
                    break;
                case MainMenuOption.StudyArea:
                    new StudyArea().Open();
                    break;
                case MainMenuOption.Exit:
                    choseExit = true;
                    break;
                default:
                    break;
            }
        }
        while (!choseExit);
    }

    public static void CreateNewStack()
    {
        Console.Clear();

        Console.WriteLine(ApplicationTexts.CREATENEWSTACK_HEADER);
        Console.WriteLine();

        var validator = new UserInputValidator();

        var newStackName = AnsiConsole.Prompt(
            new TextPrompt<string>(ApplicationTexts.CREATENEWSTACK_PROMPT)
            .Validate(validator.ConfirmUniqueStackName));

        using (var connection = DataService.OpenConnection())
        {
            string sql = "INSERT INTO Stacks(Name) VALUES (@StackName)";
            try
            {
                connection.Execute(sql, new { StackName = newStackName });

                Console.WriteLine();
                AnsiConsole.MarkupLine(string.Format(ApplicationTexts.CREATENEWSTACK_LOG, $"[cornflowerblue]{newStackName}[/]"));
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