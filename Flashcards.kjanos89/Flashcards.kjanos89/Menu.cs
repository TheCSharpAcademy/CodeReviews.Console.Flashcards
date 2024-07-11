using Spectre.Console;
namespace Flashcards.kjanos89;
public class Menu
{
    public string currentStack="no";
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
        AnsiConsole.MarkupLine("4 - [bold underline blue]Delete a flashcard[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                dbController.ChangeStack();
                break;
            case '2':
                dbController.ViewFlashcards();
                break;
            case '3':
                dbController.AddFlashcard();
                break;
            case '4':
                dbController.DeleteFlashcard();
                break;
            case '0':
                DisplayMenu();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                FlashcardMenu();
                break;
        }
    }
    public void StackMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold]Currently working in {currentStack} stack.[/]");
        AnsiConsole.MarkupLine("1 - [bold underline blue]View stacks[/]");
        AnsiConsole.MarkupLine("2 - [bold underline green]Add stack[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Change stack[/]");
        AnsiConsole.MarkupLine("4 - [bold underline blue]Delete a stack[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                dbController.ViewStacks();
                break;
            case '2':
                dbController.AddStack();
                break;
            case '3':
                dbController.ChangeStack();
                break;
            case '4':
                dbController.DeleteStack();
                break;
            case '0':
                DisplayMenu();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                StackMenu();
                break;
        }
    }
    public void StudyMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold]Currently working in {currentStack} stack.[/]");
        AnsiConsole.MarkupLine("1 - [bold underline blue]View stacks[/]");
        AnsiConsole.MarkupLine("2 - [bold underline green]Choose a Stack to study[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Start studying[/]");
        AnsiConsole.MarkupLine("4 - [bold underline blue]Check sessions[/]");
        AnsiConsole.MarkupLine("5 - [bold underline blue]Delete a study session along with its used stack and flashcards[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                dbController.ViewStacks();
                break;
            case '2':
                dbController.ChangeStack();
                break;
            case '3':
                dbController.Study();
                break;
            case '4':
                dbController.CheckSessions();
                break;
            case '5':
                dbController.DeleteSessions();
                break;
                
            case '0':
                DisplayMenu();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                StackMenu();
                break;
        }
    }

    public void QuitApplication()
    {
        Environment.Exit(2);
    }
}