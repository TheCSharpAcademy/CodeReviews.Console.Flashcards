namespace Flashcards.glaxxie.DTO;

internal record StackCreation(string Name);
internal record StackModification(int StackId, string Name);
internal record StackViewer(int StackId, string Name, int Count)
{
    public override string ToString() => Count > 0 ? $"{Name} ({Count} {(Count == 1 ? "card" : "cards")})" : Name;
};