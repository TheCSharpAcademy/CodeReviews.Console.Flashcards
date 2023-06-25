using Ohshie.FlashCards.DataAccess;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.CardsManager;

public class FlashcardEditor
{
    private readonly FlashcardsRepository _flashcardsRepository = new();
    
    public void RewriteFlashcard(FlashCardDto flashCardDto)
    {
        var updatedFlashcardName = GetName();
        var updatedFlashcardContent = AskUser.AskTextInput(where: "flashcard", what: "content");

        var flashcard = Mapper.FlashcardDtoToFlashcardMapper(flashCardDto);
        
        _flashcardsRepository.UpdateFlashcard(flashcard, updatedFlashcardName, updatedFlashcardContent);

        flashCardDto.Name = updatedFlashcardName;
        flashCardDto.Content = updatedFlashcardContent;
    }
    
    private string GetName()
    {
        string name;
        
        do
        {
            name = AskUser.AskTextInput(where: "Card", what: "name");
        } while (!Verify.EnteredAppropriateLength(name,100));

        return name;
    }
}