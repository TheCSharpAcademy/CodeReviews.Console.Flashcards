namespace vcesario.Flashcards;

public class CardObject
{
    public int Id => m_Id;
    public int StackId => m_StackId;
    public string Front => m_Front;
    public string Back => m_Back;

    private int m_Id;
    private int m_StackId;
    private string m_Front;
    private string m_Back;

    public CardObject(int Id, int StackId, string Front, string Back)
    {
        m_Id = Id;
        m_StackId = StackId;
        m_Front = Front;
        m_Back = Back;
    }
}

public class CardDTO_FrontBack
{
    private string m_Front;
    private string m_Back;

    public string Front => m_Front;
    public string Back => m_Back;

    public CardDTO_FrontBack(string Front, string Back)
    {
        m_Front = Front;
        m_Back = Back;
    }
}

public class CardDTO_IdFrontBack
{
    public int Id => m_Id;
    public string Front => m_Front;
    public string Back => m_Back;

    private int m_Id;
    private string m_Front;
    private string m_Back;

    public CardDTO_IdFrontBack(int Id, string Front, string Back)
    {
        m_Id = Id;
        m_Front = Front;
        m_Back = Back;
    }
}