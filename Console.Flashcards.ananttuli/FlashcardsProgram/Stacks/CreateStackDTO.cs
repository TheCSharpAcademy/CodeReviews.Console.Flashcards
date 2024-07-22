namespace FlashcardsProgram.Stacks;

public class CreateStackDto(
    string name
)
{
    public string Name { get; set; } = name;
}