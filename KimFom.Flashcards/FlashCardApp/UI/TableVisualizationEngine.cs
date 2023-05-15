using ConsoleTableExt;
using FlashCardApp.Data;
using FlashCardApp.Models;

namespace FlashCardApp.UI;

public class TableVisualizationEngine
{
    private static readonly IDatabaseManager DbManager = new SqlServerDatabaseManager();
    public void ViewStacks()
    {
        Console.Clear();
        
        ConsoleTableBuilder
            .From(DbManager.GetStacks())
            .WithColumn("Lessons")
            .ExportAndWriteLine();
        
        Console.WriteLine();
    }

    public void ViewFlashCards(Stack stack)
    {
        Console.Clear();
        
        ConsoleTableBuilder
            .From(DbManager.GetFlashCardsOfStack(stack))
            .WithTitle(stack.Name)
            .ExportAndWriteLine();
        
        Console.WriteLine();
    }

    public void ViewHistory()
    {
        Console.Clear();
        
        ConsoleTableBuilder.From(DbManager.GetScoresHistory()).ExportAndWriteLine();
        Console.WriteLine();
    }
}