using DatabaseLibrary.Models;
using Spectre.Console;

namespace DatabaseLibrary.Helpers;

public class StackIDValidator
{
  public static bool IsValid(List<Stack> stacks, int stackId)
  {
    if (stacks.FirstOrDefault(stack => stack.Stack_Id == stackId) == null)
    {
      AnsiConsole.Markup("[red]There is no stack with given ID. [/]");
      return false;
    }

    return true;
  }
}