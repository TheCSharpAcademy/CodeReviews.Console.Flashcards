using Spectre.Console;
namespace Flashcards.kjanos89;
public class Menu
{
    public string currentStack="no";
    DbController dbController;
    Validation validation;
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
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Study area[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Quit[/]");
        string choice = Console.ReadLine();
        validation=new Validation();
        if(validation.ValidateInputForMenu(choice))
        {
            MenuOption(choice[0]);
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Wrong input, please try again![/]");
            Thread.Sleep(1000);
            DisplayMenu();
        }
        
    }
    public void MenuOption(char option)
    {
        if (validation.ValidateInputForMenu(option))
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
        else
        {
            AnsiConsole.MarkupLine("Please try again after 1 second.");
            Thread.Sleep(1000);
            DisplayMenu();
        }
    }
    public void FlashcardMenu()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold]Currently working in {currentStack} stack.[/]");
        AnsiConsole.MarkupLine("1 - [bold underline blue]View all flashcards in stack[/]");
        AnsiConsole.MarkupLine("2 - [bold underline green]Create a new flashcard in stack[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Delete a flashcard[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                dbController.ViewFlashcards();
                break;
            case '2':
                dbController.AddFlashcard();
                break;
            case '3':
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
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Delete a stack[/]");
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
        AnsiConsole.MarkupLine("2 - [bold underline green]Start studying[/]");
        AnsiConsole.MarkupLine("3 - [bold underline yellow]Check sessions[/]");
        AnsiConsole.MarkupLine("0 - [bold red]Return to main menu[/]");
        string choice = Console.ReadLine();
        switch (choice[0])
        {
            case '1':
                dbController.ViewStacks();
                break;
            case '2':
                dbController.Study();
                break;
            case '3':
                dbController.CheckSessions();
                break;
            case '0':
                DisplayMenu();
                break;
            default:
                AnsiConsole.MarkupLine("Please try again!");
                StudyMenu();
                break;
        }
    }

    public void QuitApplication()
    {
        Environment.Exit(2);
    }
}