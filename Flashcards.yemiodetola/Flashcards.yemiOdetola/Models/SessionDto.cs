namespace Flashcards.yemiOdetola.Models;

public class SessionDto
{
  public string Name { get; }
  public Dictionary<string, int> Sessions { get; }

  public SessionDto(string name, Dictionary<string, int> sessions)
  {
    Name = name;
    Sessions = sessions;
  }
}