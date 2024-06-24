using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Reports;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;

namespace FlashcardsProgram;

public class Program
{
    public static void Main()
    {
        ConnectionManager.Init();

        var cardsRepo = new FlashcardsRepository(FlashcardDAO.TableName);
        var stacksRepo = new StacksRepository(StackDAO.TableName);
        var sessionsRepo = new StudySessionsRepository(StudySessionDAO.TableName);
        var reportsRepo = new ReportsRepository();

        var flashcardsController = new FlashcardsController(cardsRepo);

        StacksController stacksController = new StacksController(
            stacksRepo,
            flashcardsController
        );

        StudySessionsController sessionsController = new StudySessionsController(
            sessionsRepo,
            cardsRepo,
            stacksController,
            flashcardsController
        );

        ReportsController reportsController = new(reportsRepo);

        var app = new Application(stacksController, sessionsController, reportsController);

        app.Start();
    }



}