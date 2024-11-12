using Flashcards.TwilightSaw.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.TwilightSaw.Controller
{
    internal class StudyController(AppDbContext context)
    {
        public void Create(StudySession session)
        {
            context.Add(session);
            context.SaveChanges();
        }

        public StudySession? StartSession(CardStack stack, FlashcardController flashcardController)
        {
            var points = 0;
            var flashcards = flashcardController.Read(stack.CardStackId);
            if (flashcards.Count == 0)
            {
                AnsiConsole.MarkupLine($"[red]No flashcards in the Stack![/]");
                return null;
            }
            foreach (var item in flashcards)
            {
                    AnsiConsole.MarkupLine($"[yellow]{item.Front}[/]");
                    var userAnswer = UserInput.Create("Write the answer");
                    if (userAnswer == "0") break;
                    var isEqual = string.Equals(userAnswer, item.Back, StringComparison.CurrentCultureIgnoreCase);
                    if (isEqual)
                    {
                        AnsiConsole.MarkupLine("Nice! You got [green]1[/] point!");
                        points++;
                    }
                    else
                        AnsiConsole.MarkupLine($"Hell! Bad answer!\n" +
                                               $"Correct answer is - {item.Back}.");
            }
            AnsiConsole.MarkupLine($"You got {points} right answers out of {flashcards.Count}");
            return new StudySession(DateOnly.FromDateTime(DateTime.Now), points, stack.CardStackId);
        }

        public List<StudySessionDTO> Read()
        {
           return context.StudySessions.Select(s => new StudySessionDTO{Date = s.Date,Score = s.Score,Name = s.CardStack.Name}).AsNoTracking().ToList();
        }

        public void GetTable(string tableType, string date, string tableTitle)
        {
            string SqlRawQuery = @$"SELECT Name, Year, 
    ISNULL([1], 0) AS January, ISNULL([2],0) AS February,
    ISNULL([3],0) AS March, ISNULL([4],0) AS April, 
    ISNULL([5],0) AS May, ISNULL([6],0) AS June, 
    ISNULL([7],0) AS July, ISNULL([8],0) AS August, 
    ISNULL([9],0) AS September, ISNULL([10],0) AS October, 
    ISNULL([11], 0) AS November, ISNULL([12],0) AS December
FROM(
SELECT 
      MONTH(Date) AS [Month]
      ,YEAR(Date) AS [Year]
	  ,[Score]
	  ,(SELECT [Name] FROM [db].[dbo].[CardStacks] WHERE [CardStacks].[CardStackId] = [StudySessions].[CardStackId]) AS [Name]
  FROM [db].[dbo].[StudySessions]
  ) AS SourceTable
PIVOT
    (
        {tableType}(Score) 
        FOR MONTH IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])
    ) AS PivotTable WHERE YEAR LIKE '%{date}%'
ORDER BY Year;

";
            var table = new Table();
            table.Title(tableTitle);
            table.AddColumns(["Name", "Year", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"])
                .Centered();
            foreach (var session in context.Set<StudySessionTable>().FromSqlRaw(SqlRawQuery).ToList())
                session.ToTable(table);
            AnsiConsole.Write(table);
        }
    }
}
