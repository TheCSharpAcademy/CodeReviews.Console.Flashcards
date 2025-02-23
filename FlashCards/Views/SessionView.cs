using FlashCards.Models;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Views
{
    public static class SessionView
    {
        public static void ShowSessions(IEnumerable<SessionBO> sessions)
        {
            var table = new Table();
            table.AddColumn("Date");
            table.AddColumn("Score");
            table.AddColumn("MaxScore");
            table.Border(TableBorder.HeavyEdge);

            foreach (var session in sessions)
            {
                table.AddRow($"{session.Date}", $"{session.Score}", $"{session.MaxScore}");

            }
            AnsiConsole.Write(table);

        }

        public static void ShowStatistics(IEnumerable<SessionStatistics> statistics)
        {
            var table = new Table();
            table.AddColumn("Stack Name");
            table.AddColumn("January");
            table.AddColumn("February");
            table.AddColumn("March");
            table.AddColumn("April");
            table.AddColumn("May");
            table.AddColumn("June");
            table.AddColumn("July");
            table.AddColumn("August");
            table.AddColumn("September");
            table.AddColumn("October");
            table.AddColumn("November");
            table.AddColumn("December");
            table.Border(TableBorder.HeavyEdge);
            foreach (var stat in statistics)
            {
                table.AddRow($"{stat.Name}", $"{stat.January}", $"{stat.February}", $"{stat.March}", $"{stat.April}", $"{stat.May}", $"{stat.June}", $"{stat.July}", $"{stat.August}", $"{stat.September}", $"{stat.October}", $"{stat.November}", $"{stat.December}");
            }
            AnsiConsole.Write(table);
        }
    }
}