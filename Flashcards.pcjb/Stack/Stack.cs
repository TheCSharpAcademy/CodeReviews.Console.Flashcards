namespace Flashcards;

class Stack
{
    public long Id { get; }
    public string Name { get; }

    public Stack(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public Stack(string name)
    {
        Name = name;
    }
}