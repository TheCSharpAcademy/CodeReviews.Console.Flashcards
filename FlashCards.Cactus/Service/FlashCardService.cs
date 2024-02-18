using FlashCards.Cactus.Dao;
using FlashCards.Cactus.DataModel;
using FlashCards.Cactus.Helper;
using Spectre.Console;

namespace FlashCards.Cactus.Service;
public class FlashCardService
{
    public FlashCardService(FlashCardDao flashCardDao)
    {
        FlashCardDao = flashCardDao;
        FlashCards = FlashCardDao.FindAll();
    }

    public FlashCardDao FlashCardDao { get; set; }

    public List<FlashCard> FlashCards { get; set; }

    public void ShowAllFlashCards()
    {
        List<List<string>> rows = new List<List<string>>();
        FlashCards.ForEach(flashCard => rows.Add(new List<string>() { flashCard.Front, flashCard.Back }));
        ServiceHelper.ShowDataRecords(Constants.FLASHCARD, Constants.FLASHCARDS, rows);
    }

    public void AddFlashCard(List<Stack> stacks)
    {
        // Show available stack names.
        List<string> stackNames = stacks.Select(s => s.Name).ToList();
        List<List<string>> rows = new List<List<string>>();
        stackNames.ForEach(name => rows.Add(new List<string>() { name }));
        ServiceHelper.ShowDataRecords(Constants.STACK, Constants.STACKS, rows);

        // Get the stack where the new card belongs.
        string stackName = AnsiConsole.Ask<string>("Please input the [green]name[/] of the Stack where the new FlashCard belongs. Type 'q' to quit.");
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

        FlashCard flashCard = new FlashCard(sid, front, back);

        int res = FlashCardDao.Insert(flashCard);
        if (res == -1)
        {
            AnsiConsole.MarkupLine($"[red]Failed to add \"{front}, {back}\" FlashCard.[/]");
        }
        else
        {
            FlashCards.Add(flashCard);
            AnsiConsole.MarkupLine($"Successfully added [green]\"{front}, {back}\"[/] FlashCard.");
        }
    }

    public void DeleteFlashCard()
    {
        ShowAllFlashCards();

        if (FlashCards.Count == 0) return;

        string idStr = AnsiConsole.Ask<string>("Please input the [green]id[/] of the FlashCard you want to delete. Type 'q' to quit.");
        if (idStr.Equals(Constants.QUIT)) return;

        int inputId = ServiceHelper.GetUserInputId(idStr, FlashCards.Count);
        if (inputId == -1) return;

        FlashCard deletedFlashCard = FlashCards[inputId - 1];

        int res = FlashCardDao.DeleteById(deletedFlashCard.Id);
        if (res == -1)
        {
            AnsiConsole.MarkupLine($"[red]Failed to delete No.{inputId} FlashCard.[/]");
        }
        else
        {
            FlashCards = FlashCards.Where(f => f.Id != deletedFlashCard.Id).ToList();
            AnsiConsole.MarkupLine($"Successfully deleted [green]No.{inputId}[/] FlashCard.");
        }
    }

    public void UpdateFlashCard()
    {
        ShowAllFlashCards();

        int[] ids = FlashCards.Select(f => f.Id).ToArray();

        string idStr = AnsiConsole.Ask<string>("Please input the [green]id[/] of the FlashCard will be modified. Type 'q' to quit.");
        if (idStr.Equals(Constants.QUIT)) return;

        int id = ServiceHelper.GetUserInputId(idStr, FlashCards.Count);
        if (id == -1) return;

        FlashCard updatedFC = FlashCards[id - 1];

        var field = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What's [green]field[/] you want to update?")
                .PageSize(10)
                .AddChoices(new[] {
                    "Front", "Back"
                }));

        string frontBak = updatedFC.Front;
        string backBak = updatedFC.Back;

        if (field.Equals("Front"))
        {
            string front = AnsiConsole.Ask<string>("Please input the new content of FRONT. Type 'q' to quit.");
            if (front.Equals(Constants.QUIT)) return;
            updatedFC.Front = front;
        }
        else
        {
            string back = AnsiConsole.Ask<string>("Please input the new content of BACK. Type 'q' to quit.");
            if (back.Equals(Constants.QUIT)) return;
            updatedFC.Back = back;
        }

        int res = FlashCardDao.Update(updatedFC);
        if (res == -1)
        {
            updatedFC.Front = frontBak;
            updatedFC.Back = backBak;
            AnsiConsole.MarkupLine($"[red]Failed to update No.{id} FlashCard.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"Successfully update [green]No.{id} \"{updatedFC.Front}, {updatedFC.Back}\"[/] FlashCard.");
        }
    }
}

