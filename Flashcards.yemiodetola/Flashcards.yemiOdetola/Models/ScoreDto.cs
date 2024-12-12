namespace Flashcards.yemiOdetola.Models;

public class ScoreDto
{
  public string Name { get; }
  public Dictionary<string, double> Scores { get; }

  public ScoreDto(string name, Dictionary<string, double> scores)
  {
    this.Name = name;
    this.Scores = scores;
  }
}
