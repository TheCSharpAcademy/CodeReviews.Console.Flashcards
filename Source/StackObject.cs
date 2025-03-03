namespace vcesario.Flashcards;

public class StackObject
{
    public int Id => m_Id;
    public string Name => m_Name;

    private int m_Id;
    private string m_Name;

    public StackObject(int Id, string Name)
    {
        m_Id = Id;
        m_Name = Name;
    }

    public void UpdateName(string newName)
    {
        m_Name = newName;
    }
}