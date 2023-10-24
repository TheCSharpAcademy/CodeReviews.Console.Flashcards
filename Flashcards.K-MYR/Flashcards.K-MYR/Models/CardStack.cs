namespace Flashcards.K_MYR.Models;

internal class CardStack
{
    public int StackId { get; set; }

    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public DateTime Created { get; set; }

    internal void Delete()
    {
        SqlController.DeleteFlashcardsByStackID(StackId);
        SqlController.DeleteSessionsByStackID(StackId);
        SqlController.DeleteStack(StackId);
    }

    internal void Rename(string newName)
    {
        SqlController.UpdateStack($"Name = '{newName}'", StackId);
        Name = newName;
    }

    internal void UpdateNumberOfCards(int count = 1)
    {
        NumberOfCards += count;
        SqlController.UpdateStack($"NumberOfCards = {NumberOfCards}", StackId);
    }
}

internal class CardStackDto
{
    public int Row { get; set; }

    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public DateTime Created { get; set; }
}
