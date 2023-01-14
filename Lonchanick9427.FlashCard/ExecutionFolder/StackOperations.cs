using Lonchanick9427.FlashCard.DB;

namespace Lonchanick9427.FlashCard.ExcutionFolder;
public static class StackOperations
{
    public static void NewStack()
    {
        Console.Clear();
        Console.WriteLine();
        showStacks();
        Console.WriteLine("Creating a new Stack");
        Stack aux = new Stack();
        aux.Name = ToolBox.GetStringInput("Nombre");
        aux.Description = ToolBox.GetStringInput("Descripcion");
        DeckDB.Add(aux);
        Console.Clear();
        Console.WriteLine("Done!");
        showStacks();
        Console.ReadLine();
    }

    public static void DeleteStack()
    {
        List<int> i = GetStackIdList(showStacks());
        Console.WriteLine("Pick any Stack (Id) From the list");
        int aux = ToolBox.GetIntInput("Deck Id");

        while (!(i.IndexOf(aux) >= 0))
        {
            Console.WriteLine("The Stack Id provided does not exist! ");
            aux = ToolBox.GetIntInput("Deck-Id");
        }
        DeckDB.Delete(aux);
        Console.Clear();
        Console.WriteLine("Done! Here the new list!");
        showStacks();
        Console.ReadLine();
    }
    public static List<int> GetStackIdList(List<Stack> param)
    {
        List<int> i = new();
        foreach (var x in param)
            i.Add(x.Id);

        return i;
    }
    public static List<Stack> showStacks()
    {
        List<Stack> l = DeckDB.Get();
        ToolBox.DeckPrettyTable(l);
        return l;
    }
}
