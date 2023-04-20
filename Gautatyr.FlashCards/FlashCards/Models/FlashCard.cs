namespace FlashCards.Models;

public class Card
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int StackId { get; set; }
}

public class CardDto
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
}

public class CardNoId
{
    public string Question { get; set; }
    public string Answer { get; set; }
}
