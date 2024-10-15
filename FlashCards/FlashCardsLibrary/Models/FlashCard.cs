namespace FlashCardsLibrary.Models
{
    public record FlachCardCreate(string StackName,string Front,string Back);
    public record FlashCardUpdate(int ID,string Front,string Back);
    public record FlashCardDelete(int ID);
    public record FlashCardRead (int ID,string StackName,string Front,string Back);
   
}
