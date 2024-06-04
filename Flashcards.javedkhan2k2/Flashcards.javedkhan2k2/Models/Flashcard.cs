namespace Flashcards.Models;

internal class Flashcard
{
    public int Id {get;set;}
    public int StackId {get;set;}
    public string Front {get;set;} = default!;
    public string Back {get;set;} = default!;
    
}