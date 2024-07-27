namespace Flashcards.Models;

public class StackInfoDto(int Id, string Name)
{
    public int Id { get; set; } = Id;
    public string Name { get; set; } = Name;
}
