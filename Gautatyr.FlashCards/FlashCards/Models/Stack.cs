namespace FlashCards.Models;

public class Stack
{
    public int Id { get; set; }
    public string Theme { get; set; }
}

public class StackCardsDto
{
    public string Theme { get; set; }
    public List<CardDto> CardsDTO { get; set; }
}

public class StackCardsWithId
{
    public string Theme { get; set; }
    public int Id { get; set; }
}
