using FlashCards.Cactus.DataModel;

namespace FlashCards.Cactus;
public class FlashCardManagement
{
    public List<FlashCard> FlashCards { get; set; }

    public void ShowAllFlashCards()
    {
        Console.WriteLine("Show all flashCards.");
    }

    public void AddFlashCard()
    {
        Console.WriteLine("Add a new flashCard.");
    }

    public void DeleteFlashCard()
    {
        Console.WriteLine("Delete a flashcard.");
    }

    public void ModifyFlashCard()
    {
        Console.WriteLine("Modify a flashCard.");
    }
}

