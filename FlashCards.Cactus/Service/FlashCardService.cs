using FlashCards.Cactus.DataModel;
using Spectre.Console;

namespace FlashCards.Cactus.Service;
public class FlashCardService
{
    public FlashCardService()
    {
        FlashCards = new List<FlashCard>() { new FlashCard(1, 1, "Freedom", "ziyou"), new FlashCard(2, 2, "1+1=", "2") };
    }

    public List<FlashCard> FlashCards { get; set; }

    public void ShowAllFlashCards()
    {
        List<List<string>> rows = new List<List<string>>();
        FlashCards.ForEach(flashCard => rows.Add(new List<string>() { flashCard.Front, flashCard.Back }));
        DisplayHelpers.ShowDataRecords(Constants.FLASHCARD, Constants.FLASHCARDS, rows);
    }

    public void AddFlashCard()
    {
        List<Stack> stacks = new List<Stack> { new Stack(1, "Word"), new Stack(2, "Algorithm") };
        List<string> stackNames = new List<string>() { "Word", "Algorithm" };

        string stackName = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack where the new FlashCard will be stored. Type 'q' to quit.");
        if (stackName.Equals(Constants.QUIT)) return;
        while (!stackNames.Contains(stackName))
        {
            stackName = AnsiConsole.Ask<string>($"[red]{stackName}[/] Stack dose not exist. Please input a valid Stack name. Type 'q' to quit.");
            if (stackName.Equals(Constants.QUIT)) return;
        }
        int sid = stacks.Where(s => s.Name.Equals(stackName)).ToArray()[0].Id;

        string front = AnsiConsole.Ask<string>("Please input the [green]front[/] of a new FlashCard. Type 'q' to quit.");
        if (front.Equals(Constants.QUIT)) return;

        string back = AnsiConsole.Ask<string>("Please input the [green]back[/] of a new FlashCard. Type 'q' to quit.");
        if (back.Equals(Constants.QUIT)) return;

        int id = FlashCards[FlashCards.Count - 1].Id + 1;
        FlashCard flashCard = new FlashCard(id, sid, front, back);
        FlashCards.Add(flashCard);

        AnsiConsole.MarkupLine($"Successfully added [green]\"{front}, {back}\"[/] FlashCard.");
    }

    public void DeleteFlashCard()
    {
        ShowAllFlashCards();

        string idStr = AnsiConsole.Ask<string>("Please input the [green]id[/] of the FlashCard will be deleted. Type 'q' to quit.");
        if (idStr.Equals(Constants.QUIT)) return;

        int[] ids = FlashCards.Select(f => f.Id).ToArray();

        int id;
        while (!int.TryParse(idStr, out id) || !ids.Contains(id))
        {
            idStr = AnsiConsole.Ask<string>($"Please input a valid id.");
        }
        FlashCard deletedFlashCard = FlashCards.Where(f => f.Id == id).ToList()[0];
        FlashCards = FlashCards.Where(f => f.Id != id).ToList();

        AnsiConsole.MarkupLine($"Successfully deleted [green]\"{deletedFlashCard.Front}, {deletedFlashCard.Back}\"[/] FlashCard.");
    }

    public void ModifyFlashCard()
    {
        ShowAllFlashCards();

        int[] ids = FlashCards.Select(f => f.Id).ToArray();

        string idStr = AnsiConsole.Ask<string>("Please input the [green]id[/] of the FlashCard will be modified. Type 'q' to quit.");
        if (idStr.Equals(Constants.QUIT)) return;

        int id;
        while (!int.TryParse(idStr, out id) || !ids.Contains(id))
        {
            idStr = AnsiConsole.Ask<string>("Please input a valid id.");
        }
        FlashCard modifiedFC = FlashCards.Where(f => f.Id == id).ToList()[0];

        var field = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What's [green]field[/] you want to modify?")
                .PageSize(10)
                .AddChoices(new[] {
                    "Front", "Back"
                }));

        if (field.Equals("Front"))
        {
            string front = AnsiConsole.Ask<string>("Please input the new content of Front. Type 'q' to quit.");
            if (front.Equals(Constants.QUIT)) return;
            modifiedFC.Front = front;
        }
        else
        {
            string back = AnsiConsole.Ask<string>("Please input the new content of Back. Type 'q' to quit.");
            if (back.Equals(Constants.QUIT)) return;
            modifiedFC.Back = back;
        }

        AnsiConsole.MarkupLine($"Successfully modify [green]\"{modifiedFC.Front}, {modifiedFC.Back}\"[/] FlashCard.");
    }
}

