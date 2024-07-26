using Flashcards.CardView;
using Flashcards.Controllers;
using Flashcards.Models;

namespace Flashcards.StudyView;

public static class StudyViewController
{
    public static void InitMainView()
    {
        Console.Clear();
        var confirm = AnsiConsole.Confirm("Would you like to view your study sessions?");

        if (confirm) InitSessionView();
        else InitStackSelectView();
    }

    public static void InitSessionView()
    {
        Console.Clear();
        var results = StudyController.GetStudySessions();

        Table table = new();
        table.AddColumns(["Stack name", "Started at", "Ended at", "Total time"]);

        foreach (var session in results)
        {
            TimeSpan timespan = (session.EndedAt - session.StartedAt);
            table.AddRow(session.StackName, session.StartedAt.ToString(), session.EndedAt.ToString(), $"{Math.Floor(timespan.TotalHours)}H {timespan.Minutes}M");
        }

        AnsiConsole.Write(table);
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static void InitStackSelectView()
    {
        Console.Clear();
        Console.WriteLine("Please chose a stack to start studying:");

        Table table = new();
        table.AddColumns(["ID", "name"]);
        var stackList = StackController.GetAllStacks();

        foreach (var stack in stackList) table.AddRow(stack.Id.ToString(), stack.Name);

        Console.WriteLine("List of stacks:");
        AnsiConsole.Write(table);

        var stackId = CardViewController.GetMainInput(stackList, false);
        if (stackId == 0) return;

        var results = CardController.GetCardDTOList(stackId).ToArray();
        InitStudyView(stackId, results, DateTime.Now);
    }

    public static void InitStudyView(int stackId, CardDTO[] stackArray, DateTime startedAt)
    {
        Random.Shared.Shuffle<CardDTO>(stackArray);

        foreach (var card in stackArray)
        {
            Console.Clear();
            Table table = new();
            table.AddColumns(["Front"]);
            table.AddRow(card.Front);
            table.Border = TableBorder.Markdown;
            AnsiConsole.Write(table);

            Console.WriteLine("What is the answer to the question?");
            var input = Console.ReadLine() ?? "";

            if (input == card.Back) Console.WriteLine("Good job!");
            else Console.WriteLine("Incorrect. Press any key to go to the next card");
            Console.ReadKey();
        }

        var confirm = AnsiConsole.Confirm("Done studying?");

        if (confirm)
        {
            StudyController.CreateStudySession(stackId, startedAt);
            Console.WriteLine("Study session created! Press any key to continue...");
            Console.ReadKey();
        }
        else InitStudyView(stackId, stackArray, startedAt);
    }
}