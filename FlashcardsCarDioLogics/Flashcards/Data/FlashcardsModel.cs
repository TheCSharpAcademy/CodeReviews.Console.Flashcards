namespace Flashcards.Data;

public class FlashcardsModel
{
    public int FcID { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public string StackName { get; set; }

    public FlashcardsModel(int fcID, string front, string back, string stackName)
    {
        FcID = fcID;
        Front = front;
        Back = back;
        StackName = stackName;
    }
}
