using Flashcards.Eddyfadeev.Interfaces.Repositories.Operations;

namespace Flashcards.Eddyfadeev.Interfaces.Models;

/// Represents a monthly session stack.
/// It is used to store and retrieve the number of sessions for each month of the year.
/// Inherits from the `IAssignableStackName` interface.
internal interface IStackMonthlySessions : IAssignableStackName
{
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
}