using Lonchanick9427.FlashCard.DB;

namespace Lonchanick9427.FlashCard.ExcutionFolder;
public static class StackOperations
{
    public static List<Stack> showStacks2()
    {
        Dictionary<int, int> index;
        List<Stack> l = StackDB.Get(out index);
        ToolBox.DeckPrettyTable2(l);
        return l;
    }
    public static List<Stack> showStacks()
    {
        Dictionary<int, int> index = new();
        List<Stack> l = StackDB.Get(out index);
        ToolBox.DeckPrettyTable(l);
        return l;
    }
    public static List<int> GetStackIdList(List<Stack> param)
    {
        List<int> i = new();
        foreach (var x in param)
            i.Add(x.Id);

        return i;
    }
    public static void NewStack()
    {
        Console.Clear();
        Console.WriteLine();
        showStacks2();
        Console.WriteLine("Creating a new Stack");
        Stack aux = new Stack();
        aux.Name = ToolBox.GetStringInput("Nombre");
        aux.Description = ToolBox.GetStringInput("Descripcion");
        try
        {
            StackDB.Add(aux);
            Console.Clear();
            Console.WriteLine("Done!");
            showStacks2();
            Console.ReadLine();
        }
        catch (Exception ex) 
        {
            Console.WriteLine("The stack already exist. Try another name");
            Console.ReadLine();
        }
        
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
        StackDB.Delete(aux);
        Console.Clear();
        Console.WriteLine("Done! Here the new list!");
        showStacks();
        Console.ReadLine();
    }

    public static void UpdateStack()
    {
        Console.Clear();
        List<int> i = GetStackIdList(showStacks());
        Console.WriteLine("Pick any Stack (Id) From the list");
        int aux = ToolBox.GetIntInput("Stack Id");

        while (!(i.IndexOf(aux) >= 0))
        {
            Console.WriteLine("The Stack Id provided does not exist! ");
            aux = ToolBox.GetIntInput("Stack-Id");
        }
        Stack newStack = new Stack()
        {
            Id = aux,
            Name = ToolBox.GetStringInput("New Name"),
            Description = ToolBox.GetStringInput("New Description")
        };
        
        if(DB.StackDB.Update(newStack))
            Console.WriteLine("done!");
        
        Console.ReadLine();
    }
    
    
    public static void ShowStackContent()
    {
        Console.Clear();
        string op = "";
        while(op!="!")
        {
            Console.Clear();
            List<int> i = GetStackIdList(showStacks());
            Console.WriteLine("Pick any Stack (Id) From the list");
            int aux = ToolBox.GetIntInput("Stack Id");

            while (!(i.IndexOf(aux) >= 0))
            {
                Console.WriteLine("The Stack Id provided does not exist! ");
                aux = ToolBox.GetIntInput("Stack-Id");
            }
            var list = DB.CardDB.CardsByStackId(aux);
            Console.WriteLine($"This is the content of the Deck-id: {aux}");
            ToolBox.CardPrettyTable(list);
            Console.Write("Type> ! to quit or enter to go back "); op = Console.ReadLine();
        }
    }
    
}
