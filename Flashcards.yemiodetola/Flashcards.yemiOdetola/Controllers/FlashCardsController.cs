using Spectre.Console;
using Flashcards.yemiOdetola.Models;
using Flashcards.yemiOdetola.Repositories;


namespace Flashcards.yemiOdetola.Controllers;

public static class FlashCardsController
{
  private static StackRepository stackRepository = new StackRepository();
  private static FlashCardsRepository flashCardsRepository = new FlashCardsRepository();

  private static FlashCard? SelectCard(string Name)
  {
    List<FlashCard> flashcards = flashCardsRepository.GetCardsStack(Name);

    if (flashcards.Count == 0)
    {
      return null;
    }

    return AnsiConsole.Prompt(
      new SelectionPrompt<FlashCard>()
        .Title("[bold red]Select flash Card:[/]")
        .AddChoices(flashcards)
        .UseConverter(flashcard => $"{flashcards.IndexOf(flashcard) + 1} - {flashcard.Word} - {flashcard.Category}")
    );
  }

  public static void DisplayCards(string Name)
  {
    Table table = new Table()
      .Title("[bold red]Flashcards[/]")
      .AddColumn("[bold yellow]Id[/]")
      .AddColumn("[bold darkorange]Word[/]")
      .AddColumn("[bold darkorange]Category[/]");

    List<FlashCardDto> flashcardsOfStack = flashCardsRepository.GetStackCardsDto(Name);

    foreach (FlashCardDto flashcard in flashcardsOfStack)
    {
      table.AddRow(
        $"[bold yellow]{flashcardsOfStack.IndexOf(flashcard) + 1}[/]",
        $"[bold darkorange]{flashcard.Word}[/]",
        $"[bold darkorange]{flashcard.Category}[/]"
      );
    }

    AnsiConsole.Write(table);
  }

  public static void AddCard(string Name)
  {
    Stack? stack = stackRepository.GetSingleStack(Name);
    if (stack == null)
    {
      return;
    }

    string Word = AnsiConsole.Prompt(
        new TextPrompt<string>("[bold green]Insert word:[/]")
    );

    string Category = AnsiConsole.Prompt(
        new TextPrompt<string>("[bold darkorange]Insert Category:[/]")
    );

    flashCardsRepository.AddCard(stack.Id, Word, Category);

    AnsiConsole.Write(
        new Markup("[bold green]Successfully added flashcard[/]\n")
    );
  }

  public static void UpdateCard(string Name)
  {
    FlashCard? flashcard = SelectCard(Name);
    if (flashcard == null)
    {
      AnsiConsole.Write(new Markup("[bold red]Stack is empty[/]\n"));
      return;
    }

    Stack? stack = stackRepository.GetSingleStack(Name);
    if (stack == null)
    {
      return;
    }

    string newWord = AnsiConsole.Prompt(
        new TextPrompt<string>("[bold green]Insert new word:[/]")
    );

    string newCategory = AnsiConsole.Prompt(
      new TextPrompt<string>("[bold green]Insert new category:[/]")
    );

    flashCardsRepository.UpdateCard(flashcard.Id, stack.Id, newWord, newCategory);

    AnsiConsole.Write(
        new Markup("[bold green]Flashcard updated[/]\n")
    );
  }

  public static void DeleteCard(string Name)
  {
    FlashCard? flashcard = SelectCard(Name);
    if (flashcard == null)
    {
      AnsiConsole.Write(new Markup("[bold red]Stack is empty[/]\n"));
      return;
    }

    flashCardsRepository.DeleteCard(flashcard.Id);

    AnsiConsole.Write(
        new Markup("[bold green]Flashcard removed[/]\n")
    );
  }


}
