using Flashcards.kalsson;
using Flashcards.kalsson.Data;
using Flashcards.kalsson.Services;
using Flashcards.kalsson.UI;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IConfiguration config = builder.Build();
var connectionString = config.GetConnectionString("DefaultConnection");

var stackRepository = new StackRepository(connectionString);
var stackService = new StackService(stackRepository);
var stackUI = new StackUI(stackService);

var flashcardRepository = new FlashcardRepository(connectionString);
var flashcardService = new FlashcardService(flashcardRepository);
var flashcardUI = new FlashcardUI(flashcardService, stackService);

var studySessionRepository = new StudySessionRepository(connectionString);
var studySessionService = new StudySessionService(studySessionRepository);
var studySessionUI = new StudySessionUI(studySessionService, stackService);

while (true)
{
    var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose an option:")
            .AddChoices(new[] { "Show all stacks", "Add a stack", "Delete a stack", "Show all flashcards", "Add a flashcard", "Delete a flashcard", "Show all study sessions", "Add a study session", "Exit" }));

    switch (option)
    {
        case "Show all stacks":
            stackUI.ShowAllStacks();
            break;
        case "Add a stack":
            stackUI.AddStack();
            break;
        case "Delete a stack":
            stackUI.DeleteStack();
            break;
        case "Show all flashcards":
            flashcardUI.ShowAllFlashcards();
            break;
        case "Add a flashcard":
            flashcardUI.AddFlashcard();
            break;
        case "Delete a flashcard":
            flashcardUI.DeleteFlashcard();
            break;
        case "Show all study sessions":
            studySessionUI.ShowAllStudySessions();
            break;
        case "Add a study session":
            studySessionUI.AddStudySession();
            break;
        case "Exit":
            return;
    }
}