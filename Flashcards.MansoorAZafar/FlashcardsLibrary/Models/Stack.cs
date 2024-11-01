
namespace FlashcardsLibrary.Models;

internal class Stack
{
    public static string[] Headers = ["Id", "Topic"];
    public int? Id { get; set; }
    public string? Topic {get; set;}

    public Stack(string topic)
    {
        this.Topic = topic;
    }

    public Stack(int id)
    {
        this.Id = id;
    }

    public Stack(int id, string topic)
    {
        this.Id = id;
        this.Topic = topic;
    }
}