using DatabaseLibrary.Models;
using Spectre.Console;

namespace DatabaseLibrary;

public class ConsoleEngine
{
  public static string MenuSelector(string title, string[] choices)
  {
    SelectionPrompt<string> prompt = new SelectionPrompt<string>()
                                    .Title(title)
                                    .AddChoices(choices);

    string choice = AnsiConsole.Prompt(prompt);

    return choice;
  }

  public static void ShowMenuTitle(string title)
  {
    Rule rule = new Rule(title).DoubleBorder().LeftJustified();
    rule.Style = new Style(Color.Blue);
    AnsiConsole.Write(rule);
  }

  public static void ShowAppTitle()
  {
    Rule rule = new Rule("FLASHCARDS").NoBorder();
    rule.Style = new Style(Color.Blue);
    AnsiConsole.Write(rule);
  }

  public static void ShowStacksTable(List<Stack> stacks)
  {
    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Name"));

    foreach (Stack stack in stacks)
    {
      table.AddRow(stack.Stack_Id.ToString(), stack.Name);
    }

    AnsiConsole.Write(table);
  }

  public static void ShowFlashcardsTable(List<FlashcardDTO> flashcards)
  {
    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Stack Name"));
    table.AddColumn(new TableColumn("Question"));
    table.AddColumn(new TableColumn("Answer"));

    foreach (FlashcardDTO flashcard in flashcards)
    {
      table.AddRow(flashcard.Display_Id.ToString(), flashcard.Stack_Name, flashcard.Question, flashcard.Answer);
    }

    AnsiConsole.Write(table);
  }

  public static void ShowStudySessionsTable(List<StudySessionDTO> sessions)
  {
    Table table = new Table();
    table.AddColumn(new TableColumn("ID"));
    table.AddColumn(new TableColumn("Stack Name"));
    table.AddColumn(new TableColumn("Date"));
    table.AddColumn(new TableColumn("Score"));

    foreach (StudySessionDTO session in sessions)
    {
      table.AddRow(session.Id.ToString(), session.StackName, session.Date.ToString("dd-MM-yyyy"), session.Score.ToString() + "%");
    }

    AnsiConsole.Write(table);
  }
}