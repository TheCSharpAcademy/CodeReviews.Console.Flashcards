namespace Ohshie.FlashCards.DataAccess;

public class Stack
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class FlashCard
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    protected int StackId { get; set; }
}

public class StackDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<FlashCardDto>? FlashCards { get; set; }
}

public class FlashCardDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
}