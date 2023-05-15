namespace Lonchanick9427.FlashCard;

public class Card
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int Fk { get; set; }
    public Card() { }
    public Card(int id, string front, string back, int fk) 
    { 
        Id = id;
        Front = front;
        Back = back;
        Fk = fk;
    }
}
