using Spectre.Console;
using Library.Models;

namespace Flashcards.frockett;

internal class DisplayService
{
    public void DisplayAllStacks(List<StackDto> stacks, bool shouldWait = true)
    {
        Table table = new Table();
        table.AddColumn("Stacks");

        foreach (StackDto stack in stacks)
        {
            table.AddRow(stack.Name.ToString());
        }
        AnsiConsole.Write(table);

        if (shouldWait)
        {
            AnsiConsole.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }

    public void DisplayCards(List<CardDto> cards, bool shouldWait = true)
    {
        AnsiConsole.Clear();
        Table table = new Table();

        table.AddColumns(new[]
        {
            "#", "Question", "Answer"
        });

        foreach (CardDto card in cards)
        {
            int index = cards.IndexOf(card) + 1;
            table.AddRow(index.ToString(), card.Question.ToString(), card.Answer.ToString());
        }
        AnsiConsole.Write(table);

        if (shouldWait) 
        {
            AnsiConsole.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }

    public void DisplayStudySessions(List<StudySessionModel> studySessions, bool shouldWait = true)
    {
        AnsiConsole.Clear();
        Table table = new Table();

        table.AddColumns(new[]
        {
            "#", "Stack Studied", "Score", "Date and Time"
        });

        foreach (StudySessionModel studySession in studySessions)
        {
            int index = studySessions.IndexOf(studySession) + 1;
            table.AddRow(index.ToString(), studySession.StackName, studySession.Score.ToString(), studySession.SessionDateTime.ToString("f"));
        }
        AnsiConsole.Write(table);

        if (shouldWait)
        {
            AnsiConsole.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }

}
