namespace Flashcards.Models.Dtos;

public class StackDto : BaseEntity
{
    public string Name { get; init; }

    public StackDto()
    {
    }

    public StackDto(string name)
    {
        Name = name;
    }

    public StackDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}