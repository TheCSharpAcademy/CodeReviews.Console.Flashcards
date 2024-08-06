namespace Model;
public class Flashcard
{
    public int ID { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public Stack? Stacks { get; set; }
    public int StackID { get; set; }
}
