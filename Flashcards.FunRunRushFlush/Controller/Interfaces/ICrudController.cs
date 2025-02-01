using Flashcards.FunRunRushFlush.Data.Model;

namespace Flashcards.FunRunRushFlush.Controller.Interfaces;

public interface ICrudController
{
    void CreateFlashcard(Flashcard flashcard);
    void CreateStack(Stack stack);
    void CreateStudySession(StudySession studySession);
    void DeleteFlashcard(Flashcard flashcard);
    void DeleteStack(Stack stack);
    List<Flashcard> ShowAllFlashcardsOfSelectedStack(Stack stack);
    List<Stack> ShowAllStacks();
    List<StudySession> ShowAllStudySessions();
    void UpdateFlashcard(Flashcard flashcard);
    void UpdateStack(Stack stack);
}