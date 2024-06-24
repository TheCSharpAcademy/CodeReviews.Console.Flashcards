namespace FlashcardsProgram.Stacks;

public class UpdateStackDto(
    string name
)
{
    public string Name { get; set; } = name;
}