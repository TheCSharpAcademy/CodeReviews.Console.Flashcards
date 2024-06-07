namespace Flashcards.Models;

internal class FlashcardDto
{
    public int Key {get;set;}
    public int StackId {get;set;}
    public string Front {get;set;} = default!;
    public string Back {get;set;} = default!;

    public FlashcardDto(int stackId, string front, string back)
    {
        StackId = stackId;
        Front = front;
        Back = back;
    }
    
}