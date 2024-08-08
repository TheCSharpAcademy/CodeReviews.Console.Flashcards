namespace Model;

public class StackDTO
{
    public int ID { get; set; }
    public string? Name { get; set; }

    public StackDTO() { }
    public StackDTO(string? _Name)
    {
        Name = _Name;
    }
}
