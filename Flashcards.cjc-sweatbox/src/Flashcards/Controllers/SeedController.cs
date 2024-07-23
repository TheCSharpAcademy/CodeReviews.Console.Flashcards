using Flashcards.Data.Managers;
using Flashcards.Models;
using Flashcards.Services;

namespace Flashcards.Controllers;

/// <summary>
/// Controller for adding seed data to the database.
/// </summary>
public class SeedController
{
    #region Fields

    private readonly FlashcardController _flashcardController;
    private readonly StackController _stackController;
    private readonly StudySessionController _studySessionController;

    #endregion
    #region Constructors

    public SeedController(FlashcardController flashcardController, StackController stackController, StudySessionController studySessionController)
    {
        _flashcardController = flashcardController;
        _stackController = stackController;
        _studySessionController = studySessionController;
    }

    #endregion
    #region Methods

    /// <summary>
    /// Seeds the database with predefined Stacks and Flashcards.
    /// Then adds 100 study sessions randomly to the stacks, again with random datetimes and scores.
    /// Scores have a range between 0 to the amount of flashcards a stack contains.
    /// DateTimes have a range between now and the start of the previous year.
    /// </summary>
    public void SeedDatabase()
    {
        var stackSeedData = SeedDataService.GenerateStackSeedData();
        foreach (var kvp in stackSeedData)
        {
            var stackName = kvp.Key;
            var flashcardSeeds = kvp.Value;

            _stackController.AddStack(kvp.Key);
            StackDto stackDto = _stackController.GetStack(stackName);

            foreach (var flashcard in flashcardSeeds)
            {
                _flashcardController.AddFlashcard(stackDto.Id, flashcard.Question, flashcard.Answer);
            }
        }

        var studySessionDatesSeedData = SeedDataService.GenerateStudySessionDatesSeedData();
        var stacks = _stackController.GetStacks();
        var flashcards = _flashcardController.GetFlashcards();
        foreach (var dateTime in studySessionDatesSeedData)
        {
            var stack = stacks[Random.Shared.Next(0, stacks.Count)];
            var flashcardsCount = flashcards.Count(c => c.StackId == stack.Id);
            int score = Random.Shared.Next(0, flashcardsCount + 1);
            _studySessionController.AddStudySession(new StudySessionDto(stack.Id, dateTime, score));
        }
    }

    #endregion
}
