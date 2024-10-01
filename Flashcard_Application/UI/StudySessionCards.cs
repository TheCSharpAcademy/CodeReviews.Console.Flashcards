using Flashcard_Application.DataServices;
using Flashcard_Application.Models;
using Flashcards.Models;
using Flashcards.UI;
using Spectre.Console;

namespace Flashcard_Application.UI;

internal class StudySessionCards
{
    public static void SelectStackToStudy()
    {
        List<CardStack> stacks = StackDatabaseServices.GetAllStacks();
        string[] stackNameArray = new string[stacks.Count];

        for (int i = 0; i < stackNameArray.Length; i++)
        {
            stackNameArray[i] = stacks[i].StackName;
        }

        var stackSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
        .Title("[green]\n\nPlease select a stack: [/]")
                .PageSize(10)
                .AddChoices(stackNameArray)
                );

        StudySession session = new StudySession();

        List<Flashcard> studyCardList = FlashcardDatabaseServices.GetFlashCardsInStack(stackSelection);
        int totalScore = 0;
        session.SessionStartTime = DateTime.Now;

        foreach (Flashcard flashcard in studyCardList)
        {
            Console.Clear();
            AnsiConsole.Markup($"[cyan2]Question: {flashcard.Question}[/]\n\n");
            AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow]Press enter to see the Answer[/]")
                .AllowEmpty()
            );
            AnsiConsole.Markup($"[teal]Answer: {flashcard.Answer}[/]\n\n");
            Thread.Sleep(500); // .5sec pause for UX

            int score = 0;
            while (score < 1 || score > 3)
            {
                score = AnsiConsole.Prompt(
                    new TextPrompt<int>("Between [green]1[/] and [green]3[/]; how well did you know the answer?:")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid number![/]")
                        .Validate(n => n >= 1 && n <= 3));
            }
            totalScore += score;
            Thread.Sleep(1000); // 1sec pause for UX
        }
        session.SessionEndTime = DateTime.Now;
        session.SessionScore = totalScore;
        session.StackId = studyCardList[0].StackId;

        Console.Clear();
        AnsiConsole.Markup("\n\n\nEnd of Stack\n\n\n");

        StudySessionDatabaseServices.InsertStudySession(session);
        StudyAreaMenu.StudyAreaPrompt();
    }
}
