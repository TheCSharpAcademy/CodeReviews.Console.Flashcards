namespace Flashcards.Mateusz_Platek.Models;

public class StackScoreDTO
{
    public string name { get; }
    public Dictionary<string, double> scores { get; }

    public StackScoreDTO(string name, Dictionary<string, double> scores)
    {
        this.name = name;
        this.scores = scores;
    }
}