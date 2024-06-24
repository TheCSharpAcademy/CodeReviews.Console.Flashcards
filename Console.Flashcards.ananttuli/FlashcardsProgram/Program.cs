// See https://aka.ms/new-console-template for more information


using Flashcards;
using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;

// FlashcardService.Display("Spanish practice", new FlashcardDTO(1, "hola amigos in the asdasd asdasd asdasd world", "hello"), 1);
// FlashcardService.Display("Spanish practice", new FlashcardDTO(1, "hola", "hello"), 1, true);

namespace FlashcardsProgram;

public class Program
{
    public static void Main()
    {
        ConnectionManager.Init();

        var cardsRepo = new FlashcardsRepository(FlashcardDAO.TableName);
        var stacksRepo = new StacksRepository(StackDAO.TableName);
        var sessionsRepo = new StudySessionsRepository(StudySessionDAO.TableName);

        var flashcardsController = new FlashcardsController(cardsRepo);

        StacksController stacksController = new StacksController(
            stacksRepo,
            flashcardsController
        );

        StudySessionsController sessionsController = new StudySessionsController(
            sessionsRepo,
            cardsRepo,
            stacksController
        );

        var app = new Application(stacksController, sessionsController);

        app.Start();
    }



}