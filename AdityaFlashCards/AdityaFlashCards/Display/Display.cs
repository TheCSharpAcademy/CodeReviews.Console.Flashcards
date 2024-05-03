using Spectre.Console;
using AdityaFlashCards.Database.Models;

namespace AdityaFlashCards;

internal class Display
{
    internal static string GetSelection(string title, List<string> list)
    {
        var selection = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(title).AddChoices(list).HighlightStyle(new Style(foreground: Color.White)));
        return selection;
    }

    internal static void DisplayStudySessions(string[] columns, List<StudySession> allStudySessions)
    {
        var table = new Table();
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }
        foreach (StudySession session in allStudySessions)
        {
            string combinedScore = $"{session.SessionScore}/{session.TotalScore}";
            table.AddRow(session.StackName, session.SessionDate, combinedScore);
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplayFlashCards(string[] columns, List<FlashCardDTOStackView> flashCards)
    {
        var table = new Table();
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }
        foreach (FlashCardDTOStackView session in flashCards)
        {
            table.AddRow(session.PositionInStack.ToString(), session.Question, session.Answer);
        }
        AnsiConsole.Write(table);
    }

    internal static void DisplayFlashCards(string[] columns, List<FlashCardDTOFlashCardView> flashCards)
    {
        var table = new Table();
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }
        foreach (FlashCardDTOFlashCardView session in flashCards)
        {
            table.AddRow(session.FlashCardId.ToString(), session.Question, session.Answer);
        }
        AnsiConsole.Write(table);
    }
}