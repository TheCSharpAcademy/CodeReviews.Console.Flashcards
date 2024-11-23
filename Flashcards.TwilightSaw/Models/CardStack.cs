using System.ComponentModel.DataAnnotations;

namespace Flashcards.TwilightSaw.Models;

public class CardStack
{
    [Key]
    public int CardStackId { get; set; }
    public string Name { get; set; }

    public List<Flashcard> Flashcards { get; set; }
    public List<StudySession> Sessions { get; set; }

    public CardStack(string Name)
    {
        this.Name = Name;
    }

    public override string ToString()
    {
        return Name;
    }
}