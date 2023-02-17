namespace Lonchanick9427.FlashCard;

public class Stack
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Stack() { }
    public Stack(int id, string nombre, string desciption) 
    {
        Id = id;
        Name = nombre;
        Description = desciption;
    }
}
