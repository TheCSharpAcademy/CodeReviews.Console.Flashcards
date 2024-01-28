
using Library.Models;

namespace Library;

public class IdMapper
{
    Dictionary<int,int> flashcardMap = new Dictionary<int,int>();
    int sequenceIndex = 1;

    public void BuildFlashCardMap(List<CardModel> flashcards)
    {
        foreach (CardModel flashcard in flashcards)
        {
            flashcardMap.Add(sequenceIndex, flashcard.Id);
            sequenceIndex++;
        }
    }

    public int RetrieveIdFromMap(int sequenceIndex)
    {
        int cardId;

        if(flashcardMap.ContainsKey(sequenceIndex))
        {
            cardId = flashcardMap[sequenceIndex];
            flashcardMap.Clear();
            return cardId;
        }
        else
        {
            flashcardMap.Clear();
            return -1;
        }
    }

}
