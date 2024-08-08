namespace Model;
public class Study
{
    public int ID { get; set; }
    public int Attempt {  get; set; }
    public DateTime Date { get; set; }
    public int Score { get; set; }
    public string? Name { get; set; }
    public Stack? Stacks { get; set; }
    public int StackID { get; set; }
}
