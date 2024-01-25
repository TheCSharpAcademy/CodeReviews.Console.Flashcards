namespace Library.Models;

public class CardModel
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
}
