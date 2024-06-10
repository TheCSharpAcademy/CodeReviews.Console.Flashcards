using Flashcards.kalsson;
using Flashcards.kalsson.Data;
using Flashcards.kalsson.Services;
using Flashcards.kalsson.UI;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using StudySessionService = Flashcards.kalsson.StudySessionService;

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
            .AddChoices(new[] { "1. Show all stacks", "2. Add a stack", "3. Update a stack", "4. Delete a stack", 
                "5. Show all flashcards", "6. Add a flashcard", "7. Update a flashcard", "8. Delete a flashcard", 
                "9. Show all study sessions", "10. Add a study session", "11. Exit" }));

    switch (option)
    {
        case "1. Show all stacks":
            stackUI.ShowAllStacks();
            break;
        case "2. Add a stack":
            stackUI.AddStack();
            break;
        case "3. Update a stack":
            stackUI.UpdateStack();
            break;
        case "4. Delete a stack":
            stackUI.DeleteStack();
            break;
        case "5. Show all flashcards":
            flashcardUI.ShowAllFlashcards();
            break;
        case "6. Add a flashcard":
            flashcardUI.AddFlashcard();
            break;
        case "7. Update a flashcard":
            flashcardUI.UpdateFlashcard();
            break;
        case "8. Delete a flashcard":
            flashcardUI.DeleteFlashcard();
            break;
        case "9. Show all study sessions":
            studySessionUI.ShowAllStudySessions();
            break;
        case "10. Add a study session":
            studySessionUI.AddStudySession();
            break;
        case "11. Exit":
            return;
    }
}