using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Models.Dto;

namespace Flashcards.Eddyfadeev.Models.Entity;

/// <summary>
/// Represents a stack monthly session.
/// </summary>
internal class StackMonthlySessions : IStackMonthlySessions, IDbEntity<StackMonthlySessionsDto>
{
    public string? StackName { get; set; }
    public int January { get; set; }
    public int February { get; set; }
    public int March { get; set; }
    public int April { get; set; }
    public int May { get; set; }
    public int June { get; set; }
    public int July { get; set; }
    public int August { get; set; }
    public int September { get; set; }
    public int October { get; set; }
    public int November { get; set; }
    public int December { get; set; }

    /// <summary>
    /// Maps an instance of <see cref="StackMonthlySessions"/> to an instance of <see cref="StackMonthlySessionsDto"/>.
    /// </summary>
    /// <param name="stackMonthlySessions">The instance of <see cref="StackMonthlySessions"/> to convert.</param>
    /// <returns>An instance of <see cref="StackMonthlySessionsDto"/> representing the converted <see cref="StackMonthlySessions"/>.</returns>
    public StackMonthlySessionsDto MapToDto() => this.ToDto();
}