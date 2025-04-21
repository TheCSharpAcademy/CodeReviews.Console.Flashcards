namespace Flashcards.KamilKolanowski.Models;

public class UpdateStackDto
{
    public int StackId { get; set; }
    public string StackName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ColumnToUpdate { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
}
