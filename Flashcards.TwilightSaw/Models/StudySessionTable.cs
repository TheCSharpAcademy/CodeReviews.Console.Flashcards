using Flashcards.TwilightSaw.Controller;
using Spectre.Console;

namespace Flashcards.TwilightSaw.Models;

public class StudySessionTable
{
    public string Name { get; set; }
    public int Year { get; set; }
    public int January { get; set; }
    public int February { get; set; }
    public int March { get; set; }
    public int April { get; set; }
    public int May { get; set; }
    public int June { get; set; }
    public int July { get; set; }
    public int August { get; set; }
    public int September { get; set; }
    public int October { get; set; }
    public int November { get; set; }
    public int December { get; set; }

    public Table ToTable(Table table)
    {
        table.AddRow(@$"{Name}",
            $"{Year}",
            $"{January}",
            $"{February}",
            $"{March}",
            $"{April}",
            $"{May}",
            $"{June}",
            $"{July}",
            $"{August}",
            $"{September}",
            $"{October}",
            $"{November}",
            $"{December}");
        return table;
    }
}