using Flashcards.Dejmenek.Models;
using Spectre.Console;

namespace Flashcards.Dejmenek.Helpers;

public static class DataVisualizer
{
    private static readonly string[] months = {
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };

    public static void ShowFlashcards(List<FlashcardDTO> flashcards)
    {
        if (flashcards is [])
        {
            AnsiConsole.MarkupLine("No flashcards found.");
            return;
        }

        var table = new Table().Title("FLASHCARDS");
        int index = 1;

        table.AddColumn("Id");
        table.AddColumn("Front");
        table.AddColumn("Back");

        foreach (FlashcardDTO flashcard in flashcards)
        {
            table.AddRow(index.ToString(), flashcard.Front, flashcard.Back);
            index++;
        }

        AnsiConsole.Write(table);
    }

    public static void ShowStacks(List<StackDTO> stacks)
    {
        if (stacks is [])
        {
            AnsiConsole.MarkupLine("No stacks found.");
            return;
        }
        var table = new Table().Title("STACKS");

        table.AddColumn("Name");

        foreach (StackDTO stack in stacks)
        {
            table.AddRow(stack.Name);
        }

        AnsiConsole.Write(table);
    }

    public static void ShowStudySessions(List<StudySessionDTO> studySessions)
    {
        if (studySessions is [])
        {
            AnsiConsole.MarkupLine("No study sessions found.");
            return;
        }

        var table = new Table().Title("STUDY SESSIONS");
        int index = 1;

        table.AddColumn("Id");
        table.AddColumn("Date");
        table.AddColumn("Score");

        foreach (StudySessionDTO studySession in studySessions)
        {
            table.AddRow(index.ToString(), studySession.Date.ToString(), studySession.Score.ToString());
            index++;
        }

        AnsiConsole.Write(table);
    }

    public static void ShowMonthlyStudySessionReport(IEnumerable<MonthlyStudySessionsNumberData> monthlyStudySessionsNumbers)
    {
        if (!monthlyStudySessionsNumbers.Any())
        {
            AnsiConsole.MarkupLine("No study sessions found.");
            return;
        }

        var table = new Table().Title("MONTHLY STUDY SESSION");

        table.AddColumn("StackName");

        foreach (string month in months)
        {
            table.AddColumn(month);
        }

        foreach (var monthlyStudySessionNumberRecord in monthlyStudySessionsNumbers)
        {
            table.AddRow(monthlyStudySessionNumberRecord.StackName,
                monthlyStudySessionNumberRecord.JanuaryNumber.ToString(), monthlyStudySessionNumberRecord.FebruaryNumber.ToString(),
                monthlyStudySessionNumberRecord.MarchNumber.ToString(), monthlyStudySessionNumberRecord.AprilNumber.ToString(),
                monthlyStudySessionNumberRecord.MayNumber.ToString(), monthlyStudySessionNumberRecord.JuneNumber.ToString(),
                monthlyStudySessionNumberRecord.JulyNumber.ToString(), monthlyStudySessionNumberRecord.AugustNumber.ToString(),
                monthlyStudySessionNumberRecord.SeptemberNumber.ToString(), monthlyStudySessionNumberRecord.OctoberNumber.ToString(),
                monthlyStudySessionNumberRecord.NovemberNumber.ToString(), monthlyStudySessionNumberRecord.DecemberNumber.ToString()
                );
        }

        AnsiConsole.Write(table);
    }

    public static void ShowMonthlyStudySessionAverageScoreReport(IEnumerable<MonthlyStudySessionsAverageScoreData> monthlyStudySessionsAverageScores)
    {
        if (!monthlyStudySessionsAverageScores.Any())
        {
            AnsiConsole.MarkupLine("No study sessions found.");
            return;
        }

        var table = new Table().Title("MONTHLY STUDY SESSION AVERAGE SCORES");

        table.AddColumn("StackName");

        foreach (string month in months)
        {
            table.AddColumn(month);
        }

        foreach (var monthlyStudySessionAverageScoreRecord in monthlyStudySessionsAverageScores)
        {
            table.AddRow(monthlyStudySessionAverageScoreRecord.StackName,
                monthlyStudySessionAverageScoreRecord.JanuaryAverageScore.ToString(), monthlyStudySessionAverageScoreRecord.FebruaryAverageScore.ToString(),
                monthlyStudySessionAverageScoreRecord.MarchAverageScore.ToString(), monthlyStudySessionAverageScoreRecord.AprilAverageScore.ToString(),
                monthlyStudySessionAverageScoreRecord.MayAverageScore.ToString(), monthlyStudySessionAverageScoreRecord.JuneAverageScore.ToString(),
                monthlyStudySessionAverageScoreRecord.JulyAverageScore.ToString(), monthlyStudySessionAverageScoreRecord.AugustAverageScore.ToString(),
                monthlyStudySessionAverageScoreRecord.SeptemberAverageScore.ToString(), monthlyStudySessionAverageScoreRecord.OctoberAverageScore.ToString(),
                monthlyStudySessionAverageScoreRecord.NovemberAverageScore.ToString(), monthlyStudySessionAverageScoreRecord.DecemberAverageScore.ToString()
                );
        }

        AnsiConsole.Write(table);
    }

    public static void ShowFlashcardFront(FlashcardDTO flashcard)
    {
        var table = new Table();

        table.AddColumn("Front");
        table.AddRow(flashcard.Front);

        AnsiConsole.Write(table);
    }
}
