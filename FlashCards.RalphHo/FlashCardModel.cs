public class FlashCardModel
{
    private string _name="";
    private string _definition="";
    public int Id{get;set;}
    public int Position{get;set;}
    public string Name
    {
        get{return _name;}
        set{_name = value;}
    }
    public string Definition
    {
        get{return _definition;}
        set{_definition = value;}
    }

    public int StackId{get;set;}
    public string StackName{get;set;}
}