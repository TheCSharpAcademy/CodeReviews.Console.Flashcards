using Spectre.Console;

namespace cacheMe512.Flashcards.UI;

internal class MainMenu
{
    public void Show()
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Flashcard App - Main Menu[/]")
                    .PageSize(4)
                    .AddChoices(new[]
                    {
                            "Manage Stacks",
                            "Study",
                            "View Study Session Reports",
                            "Exit"
                    })
            );

            HandleOption(choice);
        }
    }

    private void HandleOption(string option)
    {
        switch (option)
        {
            case "Manage Stacks":
                var stackUI = new StackUI();
                stackUI.Show();
                break;
            case "Study":
                var studyUI = new StudySessionUI();
                studyUI.Show();
                break;
            case "View Study Session Reports":
                //var historyUI = new StudySessionHistoryUI();
                //historyUI.Show();
                break;
            case "Exit":
                AnsiConsole.MarkupLine("[bold red]Exiting application...[/]");
                Environment.Exit(0);
                break;
        }
    }

}
