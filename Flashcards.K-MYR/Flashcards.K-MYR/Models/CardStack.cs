namespace Flashcards.K_MYR.Models;

internal class CardStack
{
    public int StackId { get; set; }

    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public DateTime Created { get; set; }

    internal void Delete()
    {
        SQLController.DeleteFlashcardsByStackID(StackId);
        SQLController.DeleteSessionsByStackID(StackId);
        SQLController.DeleteStack(StackId);
    }

    internal void Rename(string newName)
    {
        SQLController.UpdateStack($"Name = '{newName}'", StackId);
        Name = newName;
    }

    internal void UpdateNumberOfCards(int count = 1)
    {
        NumberOfCards += count;
        SQLController.UpdateStack($"NumberOfCards = {NumberOfCards}", StackId);
    }
}

internal class CardStackDTO
{
    public int Row { get; set; }

    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public DateTime Created { get; set; }
}
