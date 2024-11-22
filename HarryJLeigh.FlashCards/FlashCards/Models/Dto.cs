namespace FlashCards.Data;

public record StackDto(string name);
public record FlashCardDto(int flashcardId, string front,string back);
public record StudyDto(string studyDate, int score, int flashcard_amount);