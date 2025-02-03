namespace FlashcardsLibrary.Models;

internal class Flashcard
{
    public static string[] Headers = ["Id", "FrontOfTheCard", "BackOfTheCard", "Topic"];
    public int? Id {get; set;}
    public string? FrontOfTheCard {get; set;}
    public string? BackOfTheCard {get; set;}
    public string? Topic {get; set;}

    public Flashcard(string front, string back, string topic)
    {
        this.FrontOfTheCard = front;
        this.BackOfTheCard = back;
        this.Topic = topic;
    }

    public Flashcard(int id, string front, string back, string topic)
    {
        this.Id = id;
        this.FrontOfTheCard = front;
        this.BackOfTheCard = back;
        this.Topic = topic;
    }

    public Flashcard(int id)
    {
        this.Id = id;
    }

    public Flashcard() {}
}