using FlashCards.Database;
using FlashCards.FlashCardsManager.Controllers;
using FlashCards.FlashCardsManager.Models;
using Spectre.Console;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace FlashCards.StudySessions
{
    internal class StudyController
    {
        private  StudyOptions studyOptions = new();
        private  Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        internal  void NewStudySession(Stacks stack,DataTools dataTools,StacksController stacksController)
        {
            try
            {
                StudyModel model = new();
                studyOptions.QuestionMode = config.AppSettings.Settings["QuestionMode"].Value;
                studyOptions.QuestionCount = config.AppSettings.Settings["QuestionCount"].Value == "null"  ? null : int.Parse(config.AppSettings.Settings["QuestionCount"].Value);
                studyOptions.TimerOnOff = config.AppSettings.Settings["TimerOnOff"].Value;

                model.QuestionCount = studyOptions.QuestionCount;
                model.QuestionMode = studyOptions.QuestionMode;
                model.Stack = stack.Name;

                stack.FlashCards = dataTools.GetFlashCards(stack.Name);
                if (stack.FlashCards.Count == 0) { AnsiConsole.MarkupLine("This Stacks does not contain cards."); }
                else
                {
                    if (model.QuestionCount == null)
                    {
                        Random random = new();
                        model.QuestionCount = model.QuestionMode == "Random" ?
                            random.Next(stack.NumberOfCards / 4, stack.NumberOfCards)
                            : stack.NumberOfCards;
                    }
                    stack.FlashCards = stacksController.RandomizeCards(stack);

                    Stopwatch timer = new();
                    if (studyOptions.TimerOnOff == "On") timer.Start();
                    int count = 0;
                    string? readResult;
                    Table table;
                    model.Score = 0;
                    do
                    {
                        AnsiConsole.Clear();
                        table = new();
                        table.Centered();
                        table.AddColumn(model.Stack);
                        table.AddRow(stack.FlashCards[count].Question);
                        AnsiConsole.Write(table);

                        readResult = AnsiConsole.Ask<string>("Your Answer: ");
                        if (readResult.ToLower() == stack.FlashCards[count].Answer.ToLower()) model.Score++;
                        count++;
                    } while (count < model.QuestionCount);

                    if (studyOptions.TimerOnOff == "On") { timer.Stop(); model.Time = $"{timer.Elapsed.Hours:D2}:{timer.Elapsed.Minutes:D2}:{timer.Elapsed.Seconds:D2}"; }
                    else { model.Time = null; }
                    model.Date = DateTime.Now.ToString("yyyy-MM-dd");
                    dataTools.AddStudySession(model);
                }
            }
            catch (Exception ex) { AnsiConsole.Write(ex.Message); }
        }

        internal  void SetStudyOptions(string option)
        {
            try
            {
                if (option == "Number/Mode of Questions")
                {
                    option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose an Option for Question Count")
                        .AddChoices("[red]Cancel[/]", "Fixed Number", "All Cards", "Random"));
                    switch (option)
                    {
                        case "Fixed Number":
                            config.AppSettings.Settings["QuestionCount"].Value = AnsiConsole.Ask<int>("Choose a number of question.").ToString();
                            config.AppSettings.Settings["QuestionMode"].Value = option;
                            break;
                        case "All Cards":
                            config.AppSettings.Settings["QuestionCount"].Value = "null";
                            config.AppSettings.Settings["QuestionMode"].Value = option;
                            break;
                        case "Random":
                            config.AppSettings.Settings["QuestionCount"].Value = "null";
                            config.AppSettings.Settings["QuestionMode"].Value = option;
                            break;
                        default:
                            break;
                    }
                    config.Save();
                }
                else if (option == "Timer")
                {
                    option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .AddChoices("[red]Cancel[/]", "On", "Off"));
                    if (option != "[red]Cancel[/]")
                    {
                        config.AppSettings.Settings["TimerOnOff"].Value = option;
                        config.Save();
                    }
                }
            }
            catch (Exception ex) { AnsiConsole.Write(ex.Message); }
        }

        internal  void PrintReports(string option,DataTools dataTools,StacksController stacksController)
        {
            try
            {
                if (option == "All Stacks")
                {
                    AnsiConsole.Clear();
                    List<StudyModel> sessions = dataTools.GetAllStudySessions();
                    int? totalQuestions = 0;
                    int totalScores = 0;
                    TimeSpan totalTime = new();
                    
                    foreach (StudyModel session in sessions)
                    {
                        if (session.Time == null) session.Time = "00:00";
                        totalTime = totalTime.Add(TimeSpan.ParseExact(session.Time, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None));
                        totalQuestions += session.QuestionCount;
                        totalScores += session.Score;
                    }

                    List<Stacks> stacks = stacksController.GetAllStacks(dataTools);
                    foreach (Stacks stack in stacks) PrintOneStack(stack.Name,dataTools);

                    AnsiConsole.WriteLine();
                    Table table = new();
                    table.Centered();
                    table.Border(TableBorder.Ascii);
                    table.Title("Final Resume");
                    table.AddColumns("Total Number Of session", "Total number of questions", "Total Score", "Total Time");
                    table.AddRow(sessions.Count.ToString(), totalQuestions.ToString(),totalScores.ToString(),totalTime.ToString());
                    AnsiConsole.Write(table);
                    AnsiConsole.WriteLine();
                }
                else if (option == "One Stack")
                {
                    AnsiConsole.Clear();
                    string stack = stacksController.GetStack(dataTools).Name;
                    PrintOneStack(stack,dataTools);
                };
            }
            catch (Exception ex) { AnsiConsole.Write(ex.Message); }
        }

        internal  void PrintOneStack(string stack,DataTools dataTools)
        {
            if (stack != "[red]Cancel[/]")
            {
                List<StudyModel> sessions = dataTools.GetOneStackStudySessions(stack);
                int? totalQuestions = 0;
                int totalScores = 0;
                TimeSpan totalTime = new();

                Table finalTable = new();
                finalTable.Expand();
                finalTable.Centered();
                finalTable.AddColumn(stack);
                finalTable.Columns[0].Centered();
                finalTable.Border(TableBorder.Rounded);

                Table table = new();
                table.Centered();
                table.Border(TableBorder.Rounded);
                table.Title("All Sessions");
                table.AddColumns("Date","Question Mode", "Number Of Questions", "Score", "Time");

                foreach (StudyModel session in sessions)
                {
                    if (session.Time == "00:00:00") session.Time = "Off";
                    table.AddRow(session.Date,session.QuestionMode, session.QuestionCount.ToString(), session.Score.ToString(), session.Time);
                    TimeSpan timeToAdd;
                    bool addTime = TimeSpan.TryParseExact(session.Time, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None,out timeToAdd);
                    if ((addTime))
                    {
                    totalTime = totalTime.Add(TimeSpan.ParseExact(session.Time, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None));
                    }
                    totalQuestions += session.QuestionCount;
                    totalScores += session.Score;
                }
                finalTable.AddRow(table);

                table = new();
                table.Centered();
                table.Border(TableBorder.AsciiDoubleHead);
                table.Title("Resume");
                table.AddColumns("Total Number of Sessions", "Total Number Of Questions", "Total Score", "Total Time");
                table.AddRow(sessions.Count.ToString(), totalQuestions.ToString(), totalScores.ToString(), totalTime.ToString());
                finalTable.AddRow(table);

                table = new();
                table.Centered();
                table.Border(TableBorder.DoubleEdge);
                table.Title($"Monthly Resume");
                table.AddColumn("Year-Month");

                List<StudyMonthly> monthly = dataTools.GetMonthlyReports(stack);
                int maxIndex = monthly.Count;
                for(int i = 0; i < maxIndex; i++) table.AddColumn(monthly[i].Month);

                table.AddRow("Number of Session");    
                for (int i = 0; i < maxIndex; i++) table.Rows.Update(0, i+1, new Markup($"{monthly[i].QuestionCount}"));

                finalTable.AddRow(table);
                AnsiConsole.Write(finalTable);
                AnsiConsole.WriteLine();
            }
        }

        internal  void CreateStudySessionsData(DataTools dataTools)
        {
            Random random = new();
            StudyModel study = new();
            int n;
            for (int i = 0; i < 5; i++)
            {
                n = random.Next(5, 10);
                for (int j = 0; j < n; j++)
                {
                    study.Stack = $"testStack_{i}";
                    study.Score = random.Next(0,n);
                    study.QuestionMode = "Test";
                    study.QuestionCount = n;
                    study.Time = random.Next(0, 2) == 0 ? "00:00:00" : $"00:0{n}:1{n}";
                    study.Date = DateTime.Now.AddMonths(-random.Next(0, 11)).AddDays(-random.Next(0,30)).ToString("yyyy-MM-dd");
                    dataTools.AddStudySession(study);
                }
            }
        }
    }
}

