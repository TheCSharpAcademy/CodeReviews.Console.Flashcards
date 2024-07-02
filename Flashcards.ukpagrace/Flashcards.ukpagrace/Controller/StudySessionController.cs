using Flashcards.ukpagrace.Database;
using Flashcards.ukpagrace.DTO;
using Flashcards.ukpagrace.Entity;
using Flashcards.ukpagrace.Utility;
using Flashcards.ukpagrace.Helper;
using Spectre.Console;

namespace Flashcards.ukpagrace.Controller

{
    class StudySessionController
    {
        FlashCardDatabase flashCardDatabase = new ();
        StudySessionDatabase studySessionDatabase = new ();
        StackDatabase stackDatabase = new ();
        HelperClass helper = new();
        UserInput userInput = new ();

        public void CreateTable()
        {
            studySessionDatabase.Create();
        }

        public void StartStudySession()
        {
            try
            {
                string stackName = userInput.GetStackOption();
                List<FlashcardDTO> records = new List<FlashcardDTO>();
                int stackId = stackDatabase.GetStackId(stackName);
                records = flashCardDatabase.GetFlashcards(stackId);
                if (records.Count > 0) {
                    int score = 0;

                    DateTime startDate = DateTime.Now;

                    foreach (FlashcardDTO record in records)
                    {
                        
                        var panel = new Panel(record.Question);
                        panel.Header = new PanelHeader(stackName);
                        panel.Border = BoxBorder.Double;
                        panel.BorderColor(Color.DeepPink1);
                        panel.HeaderAlignment(Justify.Center);
                        panel.Padding = new Padding(5, 5, 5, 5);

                        AnsiConsole.Write(panel);


                        var answer = AnsiConsole.Ask<string>("What's your [green]Answer[/]?");
                        bool isAnswerCorrect = studySessionDatabase.CheckAnswer(stackId, record.Question, answer);

                        if (isAnswerCorrect)
                        {
                            score++;
                        }
                        Console.Clear();
                    }
                    DateTime endDate = DateTime.Now;
                    studySessionDatabase.Insert(stackId, startDate, endDate, score);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No Flashcards to study, create some ;)[/]");
                }
            }
            catch (Exception ex)
            { 
            
                AnsiConsole.MarkupLine($"[red]{ex}[/]");
            
            }

        }

        public void ListStudySession()
        {
            List<StudySessionEntity> records = studySessionDatabase.Get();
            var table = new Table();
            table.Centered();
            table.Title("StudySessions").Centered();
            table.AddColumn(new TableColumn("[Red]id[/]").Centered());
            table.AddColumn(new TableColumn("[Red]StartDate[/]").Centered());
            table.AddColumn(new TableColumn("[Red]EndDate[/]").Centered());
            table.AddColumn(new TableColumn("[Red]Score[/]").Centered());

            if (records.Count > 0)
            {
                foreach (var record in records)
                {
                    table.AddRow(
                        new Markup($"[blue]{record.Id}[/]"),
                        new Markup($"[blue]{record.StartDate.ToString("yyyy-MM-dd hh:mm:ss")}[/]"),
                        new Markup($"[blue]{record.EndDate.ToString("yyyy-MM-dd hh:mm:ss")}[/]"),
                        new Markup($"[blue]{record.Score}[/]")
                    );
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[red] No Session Avaliable[/]");
            }
        }


        public void GetNumberofSessionPerMonth()
        {
            List <NumberOfSessionPerMonthDTO> sessions = studySessionDatabase.NumberofSessionPerMonth();
            List <string> sessionCount = new ();
            var table = new Table();
            table.Centered();
            table.Title("Number Of Session Per Month").Centered();
            for (int i = 1; i < 13; i++)
            {
                string monthName = helper.GetMonthName(i);
                table.AddColumn(new TableColumn($"[blue]{monthName}[/]").Centered());
                var session = sessions.Find(element => element.Month == i);

                if (session != null)
                {
                    sessionCount.Add($"[green]{session.Count}[/]");
                }
                else
                {
                    sessionCount.Add($"[red]{0}[/]");
                }
            }
            table.AddRow(sessionCount.Select(count => new Markup(count)).ToArray());
            AnsiConsole.Write(table);
        }

        public void GetAverageScorePerMonth()
        {
            List<AverageScorePerMonthDTO> sessions = studySessionDatabase.AverageScorePerMonth();
            List<string> sessionCount = new();
            var table = new Table();
            table.Centered();
            table.Title("Average Score Per Month").Centered();
            for (int i = 1; i < 13; i++)
            {
                string monthName = helper.GetMonthName(i);
                table.AddColumn(new TableColumn($"[blue]{monthName}[/]").Centered());
                var session = sessions.Find(element => element.Month == i);

                if (session != null)
                {
                    sessionCount.Add($"[green]{session.AverageScore}[/]");
                }
                else
                {
                    sessionCount.Add($"[red]{0}[/]");
                }
            }
            table.AddRow(sessionCount.Select(count => new Markup(count)).ToArray());
            AnsiConsole.Write(table);
        }
    }
}