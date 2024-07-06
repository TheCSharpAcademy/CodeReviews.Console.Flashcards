using Spectre.Console;
namespace Flashcards.kjanos89;
public class Menu
{
    string currentStack="no";
    DbController dbController;
    public void SetDbController(DbController _dbController)
    {
        this.dbController = _dbController;
    }
    public void DisplayMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold]Choose from the options below:[/]");
        AnsiConsole.MarkupLine("1 - [bold underline blue]Flashcards[/]");
        AnsiConsole.MarkupLine("2 - [bold underline green]Stacks[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Study[/] area");
        AnsiConsole.MarkupLine("0 - [bold red]Quit[/]");
        string choice = Console.ReadLine();
        MenuOption(choice[0]);
    }
    public void MenuOption(char option)
    {
        switch (option)
        {
            case '1':
                FlashcardMenu();
                break;
            case '2':
                StackMenu();
                break;
            case '3':
                StudyMenu();
                break;
            case '0':
                QuitApplication();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                DisplayMenu();
                break;
        }
    }
    public void FlashcardMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold]Currently working in {currentStack} stack.[/]");
        AnsiConsole.MarkupLine("1 - [bold underline blue]Change stack[/]");
        AnsiConsole.MarkupLine("2 - [bold underline green]View all flashcards in stack[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Create a new flashcard in stack[/]");
        AnsiConsole.MarkupLine("3 - [bold underline blue]Edit a flashcard[/]");
        AnsiConsole.MarkupLine("5 - [bold underline green]Delete a flashcard[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                FlashcardMenu();
                break;
            case '2':
                StackMenu();
                break;
            case '3':
                StudyMenu();
                break;
            case '4':
                StudyMenu();
                break;
            case '5':
                DisplayMenu();
                break;
            case '0':
                DisplayMenu();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                DisplayMenu();
                break;
        }
    }
    public void StackMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold]Currently working in {currentStack} stack.[/]");
        AnsiConsole.MarkupLine("1 - [bold underline blue]Change stack[/]");
        AnsiConsole.MarkupLine("2 - [bold underline green]Add stack[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Delete a stack[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                dbController.GetStack(1);
                break;
            case '2':
                dbController.AddStack();
                break;
            case '3':
                AnsiConsole.MarkupLine($"{dbController.GetStack(1)}");
                //StudyMenu();
                break;
            case '0':
                DisplayMenu();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                DisplayMenu();
                break;
        }
    }
    public void StudyMenu()
    {

    }

    public void QuitApplication()
    {
        Environment.Exit(2);
    }
}