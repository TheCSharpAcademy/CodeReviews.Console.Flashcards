namespace FlashcardsProgram.Stacks;

public class StackDAO(
    int id,
    string name
)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public static string TableName
    {
        get
        {
            return "Stacks";
        }
    }

    public override string ToString()
    {
        return Name;
    }
}