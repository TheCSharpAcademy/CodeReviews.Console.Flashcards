using System.ComponentModel.DataAnnotations;

namespace Flashcards.Eddyfadeev.Enums;

/// <summary>
/// Represents the reports menu entries.
/// </summary>
public enum ReportsMenuEntries
{
    [Display(Name = "Full Report")] 
    FullReport,
    [Display(Name = "Report By Stack")]
    ReportByStack,
    [Display(Name = "Average Yearly Report")]
    AverageYearlyReport,
    [Display(Name = "Return to Main Menu")]
    ReturnToMainMenu
}