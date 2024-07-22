using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Controllers;
using Flashcards.Arashi256.Models;
using Spectre.Console;

namespace Flashcards.Arashi256.Views
{
    internal class StudySessionView
    {
        private const int QUIT_MENU_OPTION_NUM = 5;
        private Table _tblStudySessionMenu;
        private string[] _menuOptions =
        {
            "Start a new study session",
            "List study sessions for a stack",
            "Study sessions per month report for a stack",
            "Study sessions averages per month report for a stack",
            "Return to main menu"
        };
        private StackView _stackView;
        private FlashcardController _flashcardController;
        private StudySessionController _studySessionController;

        public StudySessionView(StackView sv, FlashcardController fc)
        {
            _tblStudySessionMenu = new Table();
            _tblStudySessionMenu.AddColumn(new TableColumn("[white]CHOICE[/]").Centered());
            _tblStudySessionMenu.AddColumn(new TableColumn("[white]OPTION[/]").LeftAligned());
            for (int i = 0; i < _menuOptions.Length; i++)
            {
                _tblStudySessionMenu.AddRow($"[yellow]{i + 1}[/]", $"[darkorange]{_menuOptions[i]}[/]");
            }
            _tblStudySessionMenu.Alignment(Justify.Center);
            _flashcardController = fc;
            _stackView = sv;
            _studySessionController = new StudySessionController();
        }

        public void DisplayStudySessionMenu()
        {
            int selectedValue = 0;
            do
            {
                AnsiConsole.Write(new Text("\nS T U D Y  A R E A").Centered());
                AnsiConsole.Write(_tblStudySessionMenu);
                selectedValue = CommonUI.MenuOption($"Enter a value between 1 and {_menuOptions.Length}: ", 1, _menuOptions.Length);
                ProcessStudySessionMenu(selectedValue);
            } while (selectedValue != QUIT_MENU_OPTION_NUM);
        }

        private void ProcessStudySessionMenu(int option)
        {
            AnsiConsole.Markup($"[white]Menu option selected: {option}[/]\n");
            switch (option)
            {
                case 1:
                    // Start new study session.
                    DoStudySession();
                    CommonUI.Pause("orange4_1");
                    break;
                case 2:
                    // List study sessions.
                    ListStudySessions();
                    CommonUI.Pause("orange4_1");
                    break;
                case 3:
                    // Study sessions per month report.
                    StudySessionsPerMonthReport();
                    CommonUI.Pause("orange4_1");
                    break;
                case 4:
                    // Average study sessions per month report.
                    AverageStudySessionsPerMonthReport();
                    CommonUI.Pause("orange4_1");
                    break;
            }
        }

        private void DoStudySession()
        {
            int stackId = 0;
            StackDto? selectedStack = null;
            List<FlashcardDto> flashcards;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID to get flashcards from: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    flashcards = _flashcardController.GetAllFlashcardsForStack((int)selectedStack.Id);
                    if (flashcards.Count > 0)
                    {
                        AnsiConsole.MarkupLine($"[white]Stack subject for study:[/] [yellow]{selectedStack.Subject}...[/]");
                        AskFlashcardsQuestions(flashcards, (int)selectedStack.Id);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[white]Stack '{selectedStack.Subject}' doesn't have any flashcards![/]");
                    }
                }
            }
        }

        private void AskFlashcardsQuestions(List<FlashcardDto> flashcards, int stackid)
        {
            int totalCards = flashcards.Count;
            int score = 0;
            string answer = string.Empty;
            for(int i = 0; i < flashcards.Count;i++)
            {
                answer = AnsiConsole.Ask<string>($"[white]Flashcard:[/] [yellow]'{flashcards[i].Front}[/]'");
                if (answer.ToLower().Equals(flashcards[i].Back.ToLower()))
                {
                    AnsiConsole.MarkupLine("[white]Correct![/]");
                    score++;
                }
                else 
                {
                    AnsiConsole.MarkupLine($"[white]Incorrect![/]\n[red]The correct answer is: '{flashcards[i].Back}'[/]");
                }
                if ((i+1) < flashcards.Count)
                {
                    AnsiConsole.MarkupLine("[white]Next card...[/]");
                }
            }
            AnsiConsole.MarkupLine("[yellow]You have finished this stack.[/]");
            AnsiConsole.MarkupLine($"[yellow]Your score is:[/] [white]{score} out of {totalCards}[/]");
            StudySessionDto studySession = new StudySessionDto();
            studySession.StackId = stackid;
            studySession.TotalCards = totalCards;
            studySession.Score = score;
            studySession.DateStudied = DateTime.Now;
            studySession.Subject = _stackView.StackController.GetStack(stackid).Subject;
            if (_studySessionController.AddStudySession(studySession))
            {
                AnsiConsole.MarkupLine("[white]Study session logged![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]There was a problem logging this study session[/]");
            }
        }

        private void ListStudySessions()
        {
            int stackId = 0;
            StackDto? selectedStack = null;
            List<StudySessionDto> studysessions;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID to get study sessions for: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    studysessions = _studySessionController.GetAllStackStudySessionsForStack((int)selectedStack.Id);
                    if (studysessions.Count > 0)
                    {
                        AnsiConsole.MarkupLine($"[white]Stack subject for study sessions:[/] [yellow]{selectedStack.Subject}...[/]");
                        DisplayStudySessions(studysessions,(int)selectedStack.Id);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[white]Stack '{selectedStack.Subject}' doesn't have any study sessions![/]");
                    }
                }
            }
        }

        private void DisplayStudySessions(List<StudySessionDto> studysessions, int stackid)
        {
            string subject = _stackView.StackController.GetStack(stackid).Subject;
            Table tblSessionList = new Table();
            tblSessionList.AddColumn(new TableColumn("[yellow]ID[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[yellow]Subject[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[yellow]Date[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[yellow]Result[/]").LeftAligned());
            tblSessionList.Alignment(Justify.Center);
            if (studysessions.Count > 0)
            {
                foreach (StudySessionDto session in studysessions)
                {
                    tblSessionList.AddRow($"[white]{session.DisplayId}[/]", $"[darkorange]{subject}[/]", $"[white]{session.DateStudied.ToString("dd-MM-yyyy")}[/]", $"[white]{session.Score}/{session.TotalCards}[/]");
                }
                AnsiConsole.Write(tblSessionList);
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]Stack '{subject}' doesn't have any study sessions![/]");
            }
        }

        private void StudySessionsPerMonthReport()
        {
            int stackId = 0;
            StackDto? selectedStack = null;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    string year = GetInputYear();
                    StudySessionReportPerStackDto report = _studySessionController.StudySessionsPerMonthStackReport((int)selectedStack.Id, year);
                    if (report != null)
                        DisplayStudySessionReport($"Study Sessions Per Month Report for '{report.Subject}'", report);
                    else
                        AnsiConsole.MarkupLine("[orange1]There are no study sessions for this subject[/].");
                }
            }
        }

        private void AverageStudySessionsPerMonthReport()
        {
            int stackId = 0;
            StackDto? selectedStack = null;
            List<StackDto> stacks = _stackView.ViewStacks();
            if (stacks != null && stacks.Count > 0)
            {
                stackId = CommonUI.GetNumberInput("Please select a Stack ID: ", 0, stacks.Count);
                if (stackId == -1)
                {
                    AnsiConsole.MarkupLine("[orange1]Operation cancelled[/].");
                }
                else
                {
                    selectedStack = stacks[stackId - 1];
                    string year = GetInputYear();
                    StudySessionReportPerStackDto report = _studySessionController.StudySessionsAveragesStackReport((int)selectedStack.Id, year);
                    if (report != null)
                        DisplayStudySessionReport($"Average Study Sessions Per Month Report for '{report.Subject}'", report);
                    else
                        AnsiConsole.MarkupLine("[orange1]There are no study sessions for this subject[/].");
                }
            }
        }

        private string GetInputYear()
        {
            int currentYear = DateTime.Now.Year;
            return AnsiConsole.Prompt(
            new TextPrompt<string>("Enter a year:")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]That's not a valid year[/]")
                .Validate(year =>
                {
                    // Check if the input is a valid integer and a valid year
                    if (int.TryParse(year, out int result) && result >= 1975 && result <= currentYear)
                    {
                        return ValidationResult.Success();
                    }
                    return ValidationResult.Error("[orange1]Please enter a valid year between 1000 and 9999[/]");
                })
        );
        }

        private void DisplayStudySessionReport(string title, StudySessionReportPerStackDto report)
        {
            Table tblSessionReport = new Table();
            tblSessionReport.AddColumn(new TableColumn("[yellow]Subject[/]").LeftAligned());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Jan[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Feb[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Mar[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Apr[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]May[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Jun[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Jul[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Aug[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Sep[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Oct[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Nov[/]").Centered());
            tblSessionReport.AddColumn(new TableColumn("[yellow]Dec[/]").Centered());
            tblSessionReport.Alignment(Justify.Center);
            tblSessionReport.AddRow($"[white]{report.Subject}[/]", 
                                    $"{FormatReportValueColour((float)report.Jan)}", 
                                    $"{FormatReportValueColour((float)report.Feb)}", 
                                    $"{FormatReportValueColour((float)report.Mar)}",
                                    $"{FormatReportValueColour((float)report.Apr)}",
                                    $"{FormatReportValueColour((float)report.May)}",
                                    $"{FormatReportValueColour((float)report.Jun)}",
                                    $"{FormatReportValueColour((float)report.Jul)}",
                                    $"{FormatReportValueColour((float)report.Aug)}",
                                    $"{FormatReportValueColour((float)report.Sep)}",
                                    $"{FormatReportValueColour((float)report.Oct)}",
                                    $"{FormatReportValueColour((float)report.Nov)}",
                                    $"{FormatReportValueColour((float)report.Dec)}");
            AnsiConsole.Write(new Text(title).Centered());
            AnsiConsole.Write(tblSessionReport);
        }

        private string FormatReportValueColour(float value)
        {
            return value > 0 ? $"[lime]{value}[/]" : $"[white]{value}[/]";
        }
    }
}
