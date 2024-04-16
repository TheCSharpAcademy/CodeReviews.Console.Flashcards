using Spectre.Console;

namespace DatabaseLibrary.Helpers;

public class QuestionAndAnswerValidator
{
  public static bool IsValid(string s)
  {
    if (s.Length > 200)
    {
      AnsiConsole.Markup("[red]Questions and answers can't be longer than 200 characters.[/] ");
      return false;
    }

    return true;
  }
}