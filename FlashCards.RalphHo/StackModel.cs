public class StackModel
{
    private string _name="";
    public int Id {get;set;}
    public string Name
    {
        get{return _name;}
        set{_name = value;}
    }
    public List<FlashCardModel> FlashCards {get;set;} = new List<FlashCardModel>();
}