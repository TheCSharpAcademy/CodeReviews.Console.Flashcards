using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Repository
{
    public class StudySessionRepository
    {
        private DatabaseContext _context;

        public StudySessionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Insert(int stackId, string stackName, int score, DateTime startDate, string duration)
        {
            var session = new StudySession
            {
                StackId = stackId,
                StackName = stackName,
                Score = score,
                Date = startDate,
                Duration = duration
            };

            _context.StudySession.Add(session);
            _context.SaveChanges();

        }

        public void GetSession(string stackName)
        {
            var entities = _context.StudySession.Where(ss => ss.StackName == stackName).ToList();
            if(entities.Count == 0)
            {
                AnsiConsole.Markup("No sessions started or Stack doesnt exist");
                return;
            }

            AnsiConsole.Markup("\n[blue]Study Session Table[/]\n");
            var table = new Table();
            table.AddColumn("[green]StackName[/]");
            table.AddColumn("[green]Score[/]");
            table.AddColumn("[green]Start Time[/]");
            table.AddColumn("[green]Session Duration[/]");

            foreach(var entity in entities)
            {
                int count = _context.Flashcard.Count(f => f.StackId == entity.StackId);
                table.AddRow(
                    entity.StackName,
                    entity.Score.ToString(),
                    entity.Date.ToString(),
                    entity.Duration.Substring(0,8));
            }
            AnsiConsole.Write(table);
        }

        public void GetReport(int year)
        {
            var entities = _context.StudySession
                            .Where(ss => ss.Date.Year == year)
                            .GroupBy(ss => ss.StackName)
                            .Select(g => new
                            {
                                StackName = g.Key,
                                Jan = g.Count(s => s.Date.Month == 1),
                                Feb = g.Count(s => s.Date.Month == 2),
                                Mar = g.Count(s => s.Date.Month == 3),
                                Apr = g.Count(s => s.Date.Month == 4),
                                May = g.Count(s => s.Date.Month == 5),
                                Jun = g.Count(s => s.Date.Month == 6),
                                Jul = g.Count(s => s.Date.Month == 7),
                                Aug = g.Count(s => s.Date.Month == 8),
                                Sep = g.Count(s => s.Date.Month == 9),
                                Oct = g.Count(s => s.Date.Month == 10),
                                Nov = g.Count(s => s.Date.Month == 11),
                                Dec = g.Count(s => s.Date.Month == 12),
                            })
                            .ToList();

            if(entities.Count == 0)
            {
                AnsiConsole.Markup("\n[red]Record for following year doesn't exist\n[/]");
                return;
            }

            AnsiConsole.Markup($"\n[bold]Study Session Report for {year}[/]\n");

            var table = new Table();
            table.AddColumn("[green]Stack Name[/]");
            table.AddColumn("[green]Jan[/]");
            table.AddColumn("[green]Feb[/]");
            table.AddColumn("[green]Mar[/]");
            table.AddColumn("[green]Apr[/]");
            table.AddColumn("[green]May[/]");
            table.AddColumn("[green]Jun[/]");
            table.AddColumn("[green]Jul[/]");
            table.AddColumn("[green]Aug[/]");
            table.AddColumn("[green]Sep[/]");
            table.AddColumn("[green]Oct[/]");
            table.AddColumn("[green]Nov[/]");
            table.AddColumn("[green]Dec[/]");

            foreach (var entity in entities)
            {
                table.AddRow(
                    entity.StackName,
                    entity.Jan.ToString(),
                    entity.Feb.ToString(),
                    entity.Mar.ToString(),
                    entity.Apr.ToString(),
                    entity.May.ToString(),
                    entity.Jun.ToString(),
                    entity.Jul.ToString(),
                    entity.Aug.ToString(),
                    entity.Sep.ToString(),
                    entity.Oct.ToString(),
                    entity.Nov.ToString(),
                    entity.Dec.ToString()
                    );
            }

            AnsiConsole.Write(table);
        }
    }
}
