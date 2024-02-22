namespace FlashCards.Cactus.DataModel;
public class Stack
{
    #region Constructors

    public Stack(string name)
    {
        Name = name;
    }

    public Stack(int id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion Constructors

    #region Properties
    public int Id { get; set; }
    public string Name { get; set; }

    #endregion Properties
}

