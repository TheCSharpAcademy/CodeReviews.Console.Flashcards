using Ohshie.FlashCards.DataAccess;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.CardsManager;

public class FlashcardEditor
{
    private readonly DbOperations _dbOperations = new DbOperations();
    
    // public (string, string) RewriteFlashcard(FlashCardDto flashCardDto)
    // {
    //     var updatedFlashcardName = AskUser.AskTextInput(where: "flashcard", what: "name");
    //     var updatedFlashcardContent = AskUser.AskTextInput(where: "flashcard", what: "content");
    //
    //     var flashcard = Mapper.FlashcardDtoToFlashcardMapper(flashCardDto);
    //     
    //     _dbOperations.UpdateFlashcard(flashcard, updatedFlashcardName, updatedFlashcardContent);
    //
    //     flashCardDto.Name = updatedFlashcardName;
    //     flashCardDto.Content = updatedFlashcardContent;
    //     
    //     return (updatedFlashcardName, updatedFlashcardContent);
    // }
    
    public void RewriteFlashcard(FlashCardDto flashCardDto)
    {
        var updatedFlashcardName = AskUser.AskTextInput(where: "flashcard", what: "name");
        var updatedFlashcardContent = AskUser.AskTextInput(where: "flashcard", what: "content");

        var flashcard = Mapper.FlashcardDtoToFlashcardMapper(flashCardDto);
        
        _dbOperations.UpdateFlashcard(flashcard, updatedFlashcardName, updatedFlashcardContent);

        flashCardDto.Name = updatedFlashcardName;
        flashCardDto.Content = updatedFlashcardContent;
    }
}