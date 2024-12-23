using System.Data.Common;

public class Study
{
    private string stack;
    public string Stack
    {
    get {return stack;}
    set {stack = value;}
    }
    private int Count()
    {
        DBController.countFlashCards(DBController.ConnectDB(), stack);
        return 1;
    }  

}