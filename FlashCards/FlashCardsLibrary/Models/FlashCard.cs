namespace FlashCardsLibrary.Models
{
    public record FlashCardCreate(string StackName,string Front,string Back);
    public record FlashCardUpdate(int ID,string Front,string Back);
    public record FlashCardRead (int ID,string StackName,string Front,string Back);
   
}
