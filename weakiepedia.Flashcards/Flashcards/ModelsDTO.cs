namespace Flashcards;

internal class StackShowDTO
{
    internal string Name { get; set; }

    internal StackShowDTO(string name)
    {
        Name = name;
    }
}

internal class FlashcardShowDTO
{
    internal string Question { get; set; }
    internal string Answer { get; set; }

    internal FlashcardShowDTO(string question, string answer)
    {
        Question = question;
        Answer = answer;
    }
}

internal class FlashcardDeleteDTO
{
    internal int Id { get; set; }
    internal string Question { get; set; }
    internal string Answer { get; set; }

    internal FlashcardDeleteDTO(int id, string question, string answer)
    {
        Id = id;
        Question = question;
        Answer = answer;
    }
}
