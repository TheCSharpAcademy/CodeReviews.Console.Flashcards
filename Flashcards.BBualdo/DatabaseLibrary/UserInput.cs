using DatabaseLibrary.Helpers;
using DatabaseLibrary.Models;
using Spectre.Console;

namespace DatabaseLibrary;

public class UserInput
{
  public static string? GetStackName(List<Stack> stacks)
  {
    string stackName = AnsiConsole.Ask<string>("[yellow]Enter a name for stack[/] [blue]or type 0 to Stacks Menu: [/]");

    if (stackName == "0") return null;

    while (!StackNameValidator.IsValid(stackName, stacks))
    {
      stackName = AnsiConsole.Ask<string>("[yellow]Try again: [/]");
    }

    return stackName;
  }

  public static int? GetStackId(List<Stack> stacks)
  {
    int stackId = AnsiConsole.Ask<int>("[yellow]Enter ID of stack you want to interact with[/] [blue]or type 0 to return to Stacks Menu: [/]");

    if (stackId == 0) return null;

    while (!StackIDValidator.IsValid(stacks, stackId))
    {
      stackId = AnsiConsole.Ask<int>("[yellow]Try again: [/]");
    }

    return stackId;
  }

  public static int? GetFlashcardId(List<FlashcardDTO> flashcards)
  {
    int flashcardId = AnsiConsole.Ask<int>("[yellow]Enter ID of flashcard you want to interact with[/] [blue]or type 0 to return to Stacks Menu: [/]");

    if (flashcardId == 0) return null;

    while (!FlashcardIDValidator.IsValid(flashcards, flashcardId))
    {
      flashcardId = AnsiConsole.Ask<int>("[yellow]Try again: [/]");
    }

    return flashcardId;
  }

  public static int? GetNewStackIdForFlashcard(List<Stack> stacks)
  {
    int stackId = AnsiConsole.Ask<int>("[yellow]Enter ID of stack where flashcard should be stored.[/] [blue]or type 0 to return to Stacks Menu: [/]");

    if (stackId == 0) return null;

    while (!StackIDValidator.IsValid(stacks, stackId))
    {
      stackId = AnsiConsole.Ask<int>("[yellow]Try again: [/]");
    }

    return stackId;
  }

  public static string? GetQuestion()
  {
    string question = AnsiConsole.Ask<string>("[yellow]Enter question for flashcard (front-side): [/]");

    if (question == "0") return null;

    while (!QuestionAndAnswerValidator.IsValid(question))
    {
      question = AnsiConsole.Ask<string>("[yellow]Try again: [/]");
    }

    return question;
  }

  public static string? GetAnswer()
  {
    string answer = AnsiConsole.Ask<string>("[yellow]Enter answer for flashcard (back-side): [/]");

    if (answer == "0") return null;

    while (!QuestionAndAnswerValidator.IsValid(answer))
    {
      answer = AnsiConsole.Ask<string>("[yellow]Try again: [/]");
    }

    return answer;
  }
}