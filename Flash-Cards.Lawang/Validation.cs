using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Flash_Cards.Lawang.Models;
using Spectre.Console;

namespace Flash_Cards.Lawang;

public class Validation
{
    public Option ChooseOption(List<Option> listOfOptions, string heading, string title)
    {
        AnsiConsole.Write(new Rule($"[blue]{heading}[/]").LeftJustified().RuleStyle("red"));

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<Option>()
            .Title($"\n{title}")
            .UseConverter<Option>(c => c.Display)
            .MoreChoicesText("[grey](press UP and DOWN key to navigate)[/]")
            .AddChoices(listOfOptions)
            .HighlightStyle(Color.Blue3)
            .WrapAround()
        );

        return selection;
    }

    public string? ValidateStackName(List<Stack> listOfStack)
    {
        AnsiConsole.MarkupLine("[green bold]Give the [cyan1]Stack Name[/]?.[/]");
        AnsiConsole.MarkupLine("[grey](press '0' to go back to menu)[/]");
        string? userInput = Console.ReadLine()?.Trim();

        do
        {
            if (userInput == "0")
            {
                return null;
            }
            else if (listOfStack.FirstOrDefault(s => s.Name.ToLower() == userInput?.ToLower()) != null)
            {
                AnsiConsole.MarkupLine($"[red bold]Stack Name '{userInput}'is already present[/]");
                userInput = Console.ReadLine()?.Trim();
            }
            else if (!string.IsNullOrEmpty(userInput))
            {
                return userInput;
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value[/]");
                userInput = Console.ReadLine()?.Trim();
            }

        } while (true);
    }

    public Stack? UpdateStack(List<Stack> stackList)
    {
        if (stackList.Count() == 0)
        {
            return null;
        }
        AnsiConsole.MarkupLine("[green bold]Give the Id of the Stack you want to [yellow]Update [/]?.[/]");

        AnsiConsole.MarkupLine("[grey](press '0' to go back to menu)[/]");
        string? userInput = Console.ReadLine()?.Trim();
        int Id = 0;
        Stack? selectedStack;

        do
        {
            if (userInput == "0")
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(userInput) && int.TryParse(userInput, out Id))
            {
                selectedStack = stackList.FirstOrDefault(x => x.Id == Id);
                if (selectedStack != null)
                {
                    return selectedStack;
                }

                AnsiConsole.MarkupLine("[red bold]Stack of this Id don't exist[/]");
                userInput = Console.ReadLine()?.Trim();
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value.[/]");
                userInput = Console.ReadLine()?.Trim();
            }
        } while (true);
    }

    public Stack? UpdateStackName(Stack stack, List<Stack> stackList)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[grey](Press '0' to go back.)[/]");
        AnsiConsole.MarkupLine($"[bold]Enter the new [red]Stack Name[/] for [green]Id[/] ({stack.Id}): [/]");
        string? userInput = Console.ReadLine()?.Trim();



        do
        {
            if (userInput == "0")
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(userInput))
            {
                if (stackList.FirstOrDefault(s => s.Name == userInput) != null)
                {
                    AnsiConsole.MarkupLine($"[red bold]{userInput} already exist in Stack list.[/]");
                }
                else
                {
                    stack.Name = userInput;
                    return stack;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value.[/]");
            }

            userInput = Console.ReadLine()?.Trim();
        } while (true);
    }

    public int DeleteStack(List<Stack> stackList)
    {
        if (stackList.Count() == 0)
        {
            return 0;
        }
        AnsiConsole.MarkupLine("[green bold]Give the Id of the Stack you want to [red]Delete [/]?.[/]");

        AnsiConsole.MarkupLine("[grey](press '0' to go back to menu)[/]");
        string? userInput = Console.ReadLine()?.Trim();
        int Id;
        do
        {
            if (userInput == "0")
            {
                return 0;
            }
            else if (!string.IsNullOrEmpty(userInput) && int.TryParse(userInput, out Id))
            {
                if (stackList.Exists(s => s.Id == Id))
                {
                    return Id;
                }
                AnsiConsole.MarkupLine("[red bold]Stack of this Id don't exist[/]");
                userInput = Console.ReadLine()?.Trim();

            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value.[/]");
                userInput = Console.ReadLine()?.Trim();
            }
        } while (true);

    }

    public string? ValidateFlashCard(string title, Option? chosenStack = null)
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[grey](Press '0' to go back.)[/]");
        if(chosenStack != null)
        {
            AnsiConsole.MarkupLine($"[bold]Enter the [red]{title}[/] for [green]{chosenStack.Display}[/]: [/]");
        }
        else
        {
            AnsiConsole.Markup($"[bold]Enter the [red]{title}[/]: [/]");
        }
        string? userInput = Console.ReadLine()?.Trim();
        do
        {
            if (userInput == "0")
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(userInput))
            {
                return userInput;
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value.[/]");
                userInput = Console.ReadLine()?.Trim();
            }
        } while (true);
    }

    public FlashCardDTO? ValidateEditOrDelete(List<FlashCardDTO> flashCardDTO, string action)
    {
        AnsiConsole.MarkupLine("[grey](Press '0' to go back.)[/]");
        AnsiConsole.Markup($"[bold]Enter the [blue]S.No[/] of the flash card u want to {action}: [/]");
        string? userInput = Console.ReadLine()?.Trim();
        int serialNo = 0;
        do
        {
            if (userInput == "0")
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(userInput) && int.TryParse(userInput, out serialNo))
            {
                if (flashCardDTO.Count() >= serialNo)
                {
                    return flashCardDTO[serialNo - 1];
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red bold]S.No {serialNo} does not exist[/]");
                    userInput = Console.ReadLine()?.Trim();
                }
            }
            else
            {

                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value.[/]");
                userInput = Console.ReadLine()?.Trim();
            }

        } while (true);
    }

    public string? ValidateUserAnswer()
    {
        AnsiConsole.MarkupLine("[green]Input your answer to this card.[/]");
        AnsiConsole.MarkupLine("[grey](Press '0' to exit.)[/]");

        var answer = Console.ReadLine()?.Trim();
        do
        {
            if(answer == "0")
            {
                return null;
            }
            else if(!string.IsNullOrEmpty(answer))
            {
                return answer;
            }
            else
            {
                AnsiConsole.MarkupLine("[red bold]Please don't enter the empty or null value.[/]");
                answer = Console.ReadLine()?.Trim();
            }
        }while(true);
    }

    public int GetYear()
    {
        AnsiConsole.MarkupLine("[blue bold]--------------------------------------------[/]");
        AnsiConsole.MarkupLine("[green bold]Input a year in format 'YYYY'[/]");
        AnsiConsole.MarkupLine("[blue bold]--------------------------------------------[/]");
        AnsiConsole.MarkupLine("[grey](Press '0' to exit.)[/]");
        Console.WriteLine();

        var userInput = Console.ReadLine()?.Trim();
        DateTime year;

        do
        {
            if(userInput == "0")
            {
                return 0;
            }
            else if(DateTime.TryParseExact(userInput, "yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out year))
            {
                return year.Year;
            }

            Console.WriteLine();
            AnsiConsole.MarkupLine("[red bold]Please Enter the year in correct format[/]");
            userInput = Console.ReadLine()?.Trim();

        }while(true);
    }

}
