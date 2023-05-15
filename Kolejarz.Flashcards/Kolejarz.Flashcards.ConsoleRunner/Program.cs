using Kolejarz.Flashcards.Domain;
using Kolejarz.Flashcards.Domain.DTO;
using Spectre.Console;

var service = new FlashardsService();
await service.Seed();

while (true)
{
    AnsiConsole.Clear();

    var stacks = service.GetAllStacks();

    var prompt = new SelectionPrompt<object>().Title("What do you want [blue]to do[/]?");
    prompt.AddChoice(MenuItem.CreateNewStack);
    prompt.AddChoice(MenuItem.ViewStudySessions);
    prompt.AddChoiceGroup(new MenuItem("Select from available stacks"), stacks);
    prompt.AddChoice(MenuItem.Exit);
    var selectedOption = AnsiConsole.Prompt(prompt);

    switch (selectedOption)
    {
        case FlashcardsStackDto s: await HandleSelectedStack(s); break;
        case MenuItem i when i == MenuItem.CreateNewStack: await CreateNewStack(); break;
        case MenuItem i when i == MenuItem.ViewStudySessions: await DisplayStudySessions(); break;
        case MenuItem i when i == MenuItem.Exit: return;
        default: break;
    }
}

async Task DisplayStudySessions()
{
    AnsiConsole.Clear();

    var sessions = await service.GetAllStudySessions();
    var table = new Table().AddColumns("Date", "Topic", "Score");

    foreach(var session in sessions.OrderByDescending(s => s.SessionDate).Take(5))
    {
        var score = session.Score switch
        {
            < 0.5f => "[red]",
            >= 0.5f and < 0.75f => "[yellow]",
            >= 0.75f => "[lime]"
        };
        score += $"{Math.Round(session.Score * 100, 0)}%[/]";

        table.AddRow(
            session.SessionDate.ToString("R"),
            $"[blue]{session.Topic}[/]",
            score);
    }

    AnsiConsole.Write(table);

    Console.ReadKey();
}

async Task HandleSelectedStack(FlashcardsStackDto flashcardsStack)
{
    while (true)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"Selected [blue]{flashcardsStack}[/] topic.");
        AnsiConsole.MarkupLine($"[green]{flashcardsStack.Description}[/].");

        var prompt = new SelectionPrompt<object>().Title($"What do you want [blue]to do[/]?");
        prompt.AddChoice(MenuItem.BeginStudySession);
        prompt.AddChoice(MenuItem.CreateNewFlashcard);
        prompt.AddChoiceGroup(
            new MenuItem("Select [blue]flashcard[/]"),
            flashcardsStack.Flashcards);
        prompt.AddChoice(MenuItem.DeleteStack);
        prompt.AddChoice(MenuItem.Back);

        var selectedOption = AnsiConsole.Prompt(prompt);

        switch (selectedOption)
        {
            case MenuItem i when i == MenuItem.BeginStudySession:
                {
                    var scores = BeginStudySession(flashcardsStack);
                    await service.RecordStudySession(flashcardsStack, scores.Count(), scores.Where(s => s.answeredCorrectly).Count());
                    return;
                }
            case MenuItem i when i == MenuItem.CreateNewFlashcard:
                {
                    await CreateNewFlashcard(flashcardsStack);
                    flashcardsStack = service.GetAllStacks().Single(s => s.Name == flashcardsStack.Name);
                    break;
                }
            case FlashcardDto f:
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[blue]{f.FrontSide}[/]");
                    AnsiConsole.MarkupLine($"[yellow]{f.BackSide}[/]");
                    AnsiConsole.Write("Press any key to go back to list");
                    Console.ReadKey(true);
                    break;
                }
            case MenuItem i when i == MenuItem.DeleteStack: await service.DeleteStack(flashcardsStack); return;
            case MenuItem i when i == MenuItem.Back: return;
            default: break;
        }
    }
}

IEnumerable<(string card, bool answeredCorrectly)> BeginStudySession(FlashcardsStackDto flashcardsStackDto)
{
    AnsiConsole.Clear();

    var scores = new List<(string, bool)>();

    var fronts = flashcardsStackDto.Flashcards.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
    for(var i = 0; i < fronts.Count; i++)
    {
        var rule = new Rule($"[blue]{i + 1,2}[/][gray] / {fronts.Count,2}[/]").LeftJustified().RuleStyle("gray");
        AnsiConsole.Write(rule);

        var card = fronts[i];

        // add answers from other cards and shuffle order
        var answers = flashcardsStackDto.Flashcards
            .Except(new[] { card })
            .Select(f => f.BackSide).OrderBy(x => Guid.NewGuid()).Take(2)
            .Concat(new[] { card.BackSide }).OrderBy(x => Guid.NewGuid());

        var prompt = new SelectionPrompt<string>().Title($"{card.FrontSide}");
        prompt.AddChoices(answers);
        var selectedAnswer = AnsiConsole.Prompt(prompt);

        var correct = selectedAnswer == card.BackSide;

        AnsiConsole.MarkupLine($"[blue]{card.FrontSide}[/]");
        if (!correct)
        {
            AnsiConsole.MarkupLine($"[red]{selectedAnswer}[/]");
        }
        AnsiConsole.MarkupLine($"[green]{card.BackSide}[/]");
        AnsiConsole.WriteLine();

        scores.Add((card.FrontSide, correct));
    }

    return scores;
}

async Task CreateNewStack()
{
    var name = AnsiConsole.Ask<string>("What is the stack [blue]name[/]: ");
    var description = AnsiConsole.Ask<string>("Provide short [blue]description[/] for this stack: ");
    await service.CreateNewStack(name, description);
}

async Task<FlashcardDto> CreateNewFlashcard(FlashcardsStackDto stack)
{
    var front = AnsiConsole.Ask<string>("What is the [blue]front side[/]? ");
    var back = AnsiConsole.Ask<string>("What is the [blue]back side[/]? ");
    return await service.CreateNewFlashcard(stack, front, back);
}

internal record MenuItem(string Name)
{
    public static MenuItem CreateNewStack => new("Create new stack");
    public static MenuItem CreateNewFlashcard => new("Create new flashcard");
    public static MenuItem BeginStudySession => new("Begin study session");
    public static MenuItem ViewStudySessions => new("View study sessions");
    public static MenuItem DeleteStack => new("Delete stack");
    public static MenuItem Exit => new("Exit");
    public static MenuItem Back => new("Back");

    public override string ToString() => Name;
}