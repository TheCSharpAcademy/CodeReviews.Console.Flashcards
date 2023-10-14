namespace Flashcards.K_MYR.Models;

internal class CardStack
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public DateTime CreatedDate { get; set; }

    internal void Delete()
    {
        SQLController.DeleteStack(this.Id);
    }

    internal void Rename(string newName)
    {
        SQLController.UpdateStack(newName, Id);
        Name = newName;
    }
}

internal class CardStackDTO
{
    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public DateTime CreatedDate { get; set; }
}
