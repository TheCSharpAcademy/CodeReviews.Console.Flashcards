namespace Flashcards.Models;

internal class StackYearlyReport
{
    public string? January { get; set; } = default!;
    public string? February { get; set; } = default!;
    public string? March { get; set; } = default!;
    public string? April { get; set; } = default!;
    public string? May { get; set; } = default!;
    public string? June { get; set; } = default!;
    public string? July { get; set; } = default!;
    public string? August { get; set; } = default!;
    public string? September { get; set; } = default!;
    public string? October { get; set; } = default!;
    public string? November { get; set; } = default!;
    public string? December { get; set; } = default!;
}

internal class AllStackYearlyReport : StackYearlyReport
{
    public string? StackName { get; set; } = default!;
}