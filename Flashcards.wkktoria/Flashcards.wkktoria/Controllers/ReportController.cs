using System.Globalization;
using Flashcards.wkktoria.Models.Dtos;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.Services.Helpers;
using Flashcards.wkktoria.UserInteractions;
using Flashcards.wkktoria.UserInteractions.Helpers;

namespace Flashcards.wkktoria.Controllers;

internal class ReportController
{
    private readonly List<ReportDataDto> _reportData;
    private readonly ReportDataService _reportDataService;

    internal ReportController(StackService stackService, SessionService sessionService,
        ReportDataService reportDataService)
    {
        _reportDataService = reportDataService;
        _reportData = ReportDataHelper.Load(stackService, sessionService);
    }

    internal void Sessions()
    {
        Console.Clear();

        var created = _reportDataService.Create(_reportData);

        if (created)
        {
            var sessionsToShow = new List<ReportDataDto.ReportDataSessions>();

            var year = UserInput.GetNumberInput("Enter year to show report.");

            if (_reportData.Any(rp => rp.SessionYear == year))
            {
                for (var month = 1; month <= 12; month++)
                {
                    var sessions = _reportDataService.GetSessionsInMonth(month, year);

                    sessionsToShow.Add(new ReportDataDto.ReportDataSessions
                    {
                        SessionMonth = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month),
                        Sessions = sessions
                    });
                }

                TableVisualisation.ShowSessionsInYear(sessionsToShow, year);
            }
            else
            {
                UserOutput.InfoMessage("No data in selected year.");
            }
        }
        else
        {
            UserOutput.ErrorMessage("Failed to create record data.");
        }


        if (_reportData.Any())
        {
            var deleted = _reportDataService.DeleteAll();

            if (!deleted) UserOutput.ErrorMessage("Failed to delete record data.");
        }


        ConsoleHelpers.PressToContinue();
    }

    internal void AverageScores()
    {
        Console.Clear();

        var created = _reportDataService.Create(_reportData);

        if (created)
        {
            var sessionsToShow = new List<ReportDataDto.ReportDataAverageScores>();

            var year = UserInput.GetNumberInput("Enter year to show report.");

            if (_reportData.Any(rp => rp.SessionYear == year))
            {
                var stackNames = _reportData.Select(rp => rp.StackName).Distinct().ToList();

                for (var month = 1; month <= 12; month++)
                    sessionsToShow.AddRange(from name in stackNames
                        let score = _reportDataService.GetAverageScoreInMonth(month, year, name)
                        select new ReportDataDto.ReportDataAverageScores
                        {
                            SessionMonth = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month),
                            StackName = name, Score = score
                        });

                TableVisualisation.ShowAverageScoresInYear(sessionsToShow, year);
            }
            else
            {
                UserOutput.InfoMessage("No data in selected year.");
            }
        }
        else
        {
            UserOutput.ErrorMessage("Failed to create report data.");
        }

        if (_reportData.Any())
        {
            var deleted = _reportDataService.DeleteAll();

            if (!deleted) UserOutput.ErrorMessage("Failed to delete record data.");
        }

        ConsoleHelpers.PressToContinue();
    }
}