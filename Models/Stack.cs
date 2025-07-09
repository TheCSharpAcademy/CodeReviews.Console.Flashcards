namespace DotNETConsole.Flashcards.Models;

public class Stack
{
    public int ID { get; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
