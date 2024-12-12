using Flashcards.yemiOdetola.Models;
using Flashcards.yemiOdetola.Repositories;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.yemiOdetola.Controllers;

public static class StackController
{
  private static StackRepository stackRepository = new StackRepository();

  public static Stack? SelectStack()
  {
    List<Stack> stacks = stackRepository.GetStacks();
    if (stacks.Count == 0)
    {
      return null;
    }

    return AnsiConsole.Prompt(
        new SelectionPrompt<Stack>()
            .Title("[bold red] Select stack:[/]")
            .AddChoices(stacks)
            .UseConverter(stack => $"{stack.Name}")

    );
  }

  public static void DisplayStacks()
  {
    Table table = new Table()
        .Title("[bold red]Stacks[/]")
        .HideHeaders()
        .AddColumn("Name");

    List<Stack> stacks = stackRepository.GetStacks();
    foreach (Stack stack in stacks)
    {
      table.AddRow($"[bold purple]{stack.Name}[/]");
    }

    AnsiConsole.Write(table);
  }

  public static void AddStack()
  {
    string name = AnsiConsole.Prompt(
        new TextPrompt<string>("[bold purple]Insert stack name:[/]")
    );

    try
    {
      stackRepository.CreateStack(name);
    }
    catch (SqlException)
    {
      AnsiConsole.Write(
          new Markup("[bold red]Stack already exists[/]\n")
      );
      return;
    }
    AnsiConsole.Write(
        new Markup("[bold green]New Stack has been added[/]\n")
    );
  }

  public static void UpdateStack()
  {
    Stack? stack = SelectStack();
    if (stack == null)
    {
      AnsiConsole.Write(
          new Markup("[bold red]List of stacks is empty[/]")
      );
      return;
    }

    string newName = AnsiConsole.Prompt(
        new TextPrompt<string>("[bold purple]Insert the new stack name:[/]")
    );

    try
    {
      stackRepository.UpdateStack(stack.Name, newName);
    }
    catch (SqlException)
    {
      AnsiConsole.Write(
          new Markup("[bold red]Stack already exists[/]\n")
      );
      return;
    }

    AnsiConsole.Write(
        new Markup("[bold green]Stack updated successfully[/]\n")
    );
  }


  public static void DeleteStack()
  {
    Stack? stack = SelectStack();
    if (stack == null)
    {
      AnsiConsole.Write(
          new Markup("[bold red]List of stacks is empty[/]")
      );
      return;
    }

    stackRepository.DeleteStack(stack.Name);

    AnsiConsole.Write(
        new Markup("[bold green]Stack removed[/]\n")
    );
  }

}
