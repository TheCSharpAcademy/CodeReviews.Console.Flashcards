using FlashCards.kwm0304.Models;
using FlashCards.kwm0304.Repositories;
using FlashCards.kwm0304.Services;
using Spectre.Console;

namespace FlashCards.kwm0304.Views;

public class ReportUI
{
  private readonly StudySessionRepository _repository;
  public ReportUI()
  {
    _repository = new StudySessionRepository();
  }
  public async void HandleReports()
  {
    string choice = SelectionPrompt.ReportsSelection();
    if (choice == "View monthly score")
    {
      List<ReportData> reports = await _repository.GetReportByScore();
            DisplayReport("score", reports);
    }
    else if (choice == "View monthly attempts")
    {
      List<ReportData> reports = await _repository.GetReportsByAttempt();
            DisplayReport("attempts", reports);
    }
    else if (choice == "Back")
    {
      //implement
      AnsiConsole.WriteLine("todo");
    }
  }

  private static void DisplayReport(string input, List<ReportData> reports)
  {
    AnsiConsole.WriteLine("Press any key to return home\n");
    string[] months =
    [
      "Jan", "Feb", "Mar", "Apr", "May", "Jun",
      "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];
    var table = new Table();
    table.Title($"Average {input} Per Month");
    table.AddColumns(months);
    foreach (var report in reports)
    {
      table.AddRow(
        report.StackName ?? "n/a",
        report.Jan?.ToString() ?? "n/a",
        report.Feb?.ToString() ?? "n/a",
        report.Mar?.ToString() ?? "n/a",
        report.Apr?.ToString() ?? "n/a",
        report.May?.ToString() ?? "n/a",
        report.Jun?.ToString() ?? "n/a",
        report.Jul?.ToString() ?? "n/a",
        report.Aug?.ToString() ?? "n/a",
        report.Sep?.ToString() ?? "n/a",
        report.Oct?.ToString() ?? "n/a",
        report.Nov?.ToString() ?? "n/a",
        report.Dec?.ToString() ?? "n/a"
      );
    }
    AnsiConsole.Write(table);
    Console.ReadKey();
  }
}