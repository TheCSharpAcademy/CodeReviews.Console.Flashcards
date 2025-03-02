namespace vcesario.Flashcards;

public class CardDTO
{
    private string m_Front;
    private string m_Back;

    public string Front => m_Front;
    public string Back => m_Back;

    public CardDTO(string Front, string Back)
    {
        m_Front = Front;
        m_Back = Back;
    }
}