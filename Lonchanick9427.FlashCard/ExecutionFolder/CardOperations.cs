using Lonchanick9427.FlashCard.DB;

namespace Lonchanick9427.FlashCard.ExcutionFolder;
public static class CardOperations
{
    public static List<int> GetCardIdList(List<Card> param)
    {
        List<int> i = new();
        foreach (var x in param)
            i.Add(x.Id);

        return i;
    }
    public static void NewCard()
    {
        Console.Clear();
        List<int> i = StackOperations.GetStackIdList(StackOperations.showStacks());
        Console.WriteLine("Pick any Stack (Id) From the list");
        int stackId = ToolBox.GetIntInput("Deck Id");

        while (!(i.IndexOf(stackId) >= 0))
        {
            Console.WriteLine("The Stack Id provided does not exist! ");
            stackId = ToolBox.GetIntInput("Deck-Id");
        }

        Console.WriteLine($"Creating a new Card For stack Id: {stackId}");
        Card aux = new();
        aux.Front = ToolBox.GetStringInput("Front-Face");
        aux.Back = ToolBox.GetStringInput("Back-Face");
        aux.Fk = stackId;

        CardDB.Add(aux);
        Console.WriteLine("Done!");
        Console.ReadLine();
    }

    public static void DeleteCard()
    {
        Console.Clear();

        List<int> i = StackOperations.GetStackIdList(StackOperations.showStacks());
        Console.WriteLine("Pick any Stack (Id) From the list");
        int id = ToolBox.GetIntInput("Stack Id");
        while (!(i.IndexOf(id) >= 0))
        {
            Console.WriteLine("The Stack Id provided does not exist! ");
            id = ToolBox.GetIntInput("Deck-Id");
        }
        Console.Clear();
        Console.WriteLine($"This cards belong to Stack Id: {id}");
        List<Card> list = CardDB.CardsByStackId(id);
        ToolBox.CardPrettyTable(list);
        Console.Write("Type an Falsh Card Id to delete. ");
        int id2 = ToolBox.GetIntInput("");
        List<int> idList = GetCardIdList(list);
        while (!(idList.IndexOf(id2) >= 0))
        {
            Console.WriteLine("The Card Id provided does not exist! ");
            id2 = ToolBox.GetIntInput("Card-Id");
        }
        if (CardDB.Delete(id2))
            Console.Write("done!");

        Console.ReadLine();
    }
}
