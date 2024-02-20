namespace Flashcards.Mateusz_Platek.Models;

public class StackSessionDTO
{
    public string name { get; }
    public Dictionary<string, int> sessions { get; }

    public StackSessionDTO(string name, Dictionary<string, int> sessions)
    {
        this.name = name;
        this.sessions = sessions;
    }
}