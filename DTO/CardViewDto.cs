namespace DotNETConsole.Flashcards.DTO;


public class CardViewDto
{
    public int ID { get; }
    public string Question { get; }
    public string Answer { get; }
    public string Stack { get; }

    public override string ToString()
    {
        return $"{Question} - {Stack}";
    }
}
