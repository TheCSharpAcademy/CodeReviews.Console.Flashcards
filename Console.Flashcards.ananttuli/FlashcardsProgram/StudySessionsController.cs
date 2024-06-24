using FlashcardsProgram.Database;
using FlashcardsProgram.Flashcards;
using FlashcardsProgram.Stacks;
using FlashcardsProgram.StudySession;
using Spectre.Console;

namespace FlashcardsProgram;

public class StudySessionsController(
    StudySessionsRepository sessionsRepo,
    FlashcardsRepository cardsRepo,
    StacksController stacksController,
    FlashcardsController cardsController
)
{
    public void ShowSessionsList()
    {
        List<StudySessionDAO> sessions = sessionsRepo.List();

        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine(Utils.Text.Markup("No sessions", "red"));
            Utils.ConsoleUtil.PressAnyKeyToClear();
            return;
        }

        var table = new Table();

        table.AddColumns(["Date", "Num correct", "Num attempted"]);

        for (int i = 0; i < sessions.Count; i++)
        {
            table.AddRow([
                sessions[i].DateTime.ToString("g"),
                sessions[i].NumCorrect.ToString(),
                sessions[i].NumAttempted.ToString()
            ]);
        }

        AnsiConsole.Write(table);
    }

    public void Study()
    {
        var selectedStack = stacksController.SelectStackFromList();

        if (selectedStack == null)
        {
            return;
        }

        var (isValidNumCards, numCards) = ReadValidNumCardsForSession(selectedStack);

        if (!isValidNumCards)
        {
            return;
        }

        AnsiConsole.MarkupLine($"Starting session with {numCards} cards.");

        var cards = cardsRepo.List(numCards);

        int numAttempted = 0;
        int numCorrect = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            bool isResponseCorrect = PlayCard(selectedStack, cards[i], i + 1);

            numAttempted++;
            numCorrect += isResponseCorrect ? 1 : 0;

            var key = Utils.ConsoleUtil.PressAnyKeyToClear(
                "Press [red]0 to exit[/] or [blue]another key to continue[/] session."
            );

            if ($"{key.KeyChar}".ToLower().Equals("0"))
            {
                break;
            }
        }

        CreateStudySessionForStack(selectedStack, numCorrect, numAttempted);
    }

    private bool PlayCard(StackDAO selectedStack, FlashcardDAO card, int order)
    {

        cardsController.DisplayCard(selectedStack.Name, FlashcardMapping.ToDTO(card), order);

        var response = AnsiConsole.Ask<string>("Response?");

        var isResponseCorrect = response.ToLower().Trim().Equals(card.Back.ToLower().Trim());

        AnsiConsole.MarkupLine(
            isResponseCorrect ?
                "[green]Correct! +1[/]" :
                $"[red]Incorrect. The answer was '{card.Back}'[/]"
        );

        return isResponseCorrect;
    }

    private Tuple<bool, int> ReadValidNumCardsForSession(StackDAO selectedStack)
    {
        int numCardsInStack = cardsRepo.GetNumCardsInStack(selectedStack.Id);
        if (numCardsInStack < 1)
        {
            AnsiConsole.MarkupLine(
                $"[red]No cards in stack.[/]Please [blue]add cards[/] to practice with this stack."
            );

            return new Tuple<bool, int>(false, 0);
        }

        int numCards = AnsiConsole.Prompt(
            new TextPrompt<int>($"How many cards for this session? (Min: 1, Max: {numCardsInStack})")
                .Validate(input =>
                {
                    if (input < 1 || input > numCardsInStack)
                    {
                        return ValidationResult.Error(
                            $"Stack has {numCardsInStack} card(s)." +
                            $"Please enter value between 1 & {numCardsInStack} to start session."
                        );
                    }

                    return ValidationResult.Success();
                }
            )
        );

        return new Tuple<bool, int>(true, numCards);
    }



    private void CreateStudySessionForStack(StackDAO stack, int numCorrect, int numAttempted)
    {
        AnsiConsole.MarkupLine($"You scored {numCorrect}/{numAttempted}");

        sessionsRepo.Create(
            new CreateStudySessionDTO(
                numAttempted: numAttempted,
                numCorrect: numCorrect,
                dateTime: DateTime.Now,
                stackId: stack.Id
            )
        );
    }
}

public static class StudySessionMenuChoice
{
    public const string EditStackName = "[blue]Edit[/] Stack Name";
    public const string DeleteStack = "[blue]Delete[/] Stack";
    public const string AddFlashcard = "Add new [yellow]Flashcard[/]";
    public const string VEDFlashcard = "Manage [yellow]flashcards[/]";
    public const string Back = "[red]<- Go back[/]";
}