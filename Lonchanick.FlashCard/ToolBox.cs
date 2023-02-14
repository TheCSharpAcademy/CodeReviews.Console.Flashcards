using ConsoleTables;
namespace Lonchanick9427.FlashCard;

public class ToolBox
{
    public static string RemoveWhitespace(string str)
    {
        return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }

    public static string GetStringInput(string param)
    {
        Console.Write($"{param}: ");
        string value = Console.ReadLine();

        while (string.IsNullOrEmpty(value))
        {
            Console.WriteLine("Empty values are not allowed, Try again");
            Console.Write($"{param}: ");
            value = Console.ReadLine();
        }
        return value;
    }
    public static int GetIntInput(string param)
    {
        Console.Write($"{param}: "); string readed = Console.ReadLine();
        int aux;
        while (!int.TryParse(readed, out aux))
        {
            Console.Write($"Error: {readed} is not a valid input! try again: ");
            readed = Console.ReadLine();
        }

        return aux;
    }

    public static void DeckPrettyTable(List<Stack> records)
    {
        var table = new ConsoleTable("Id", "Name", "Description");
        foreach (var item in records)
            table.AddRow(item.Id, item.Name, item.Description);
        
        Console.WriteLine("\t DECK-LIST");
        table.Write(ConsoleTables.Format.MarkDown);
    }

    public static void DeckPrettyTable2(List<Stack> records/*, Dictionary<int,int> i*/)
    {
        int aux = 1;
        var table = new ConsoleTable("Id", "Name", "Description");
        foreach (var item in records)
        { table.AddRow(aux, item.Name, item.Description); aux++; }
        
        Console.WriteLine("\t DECK-LIST");
        table.Write(ConsoleTables.Format.MarkDown);
    }

    public static void CardPrettyTable(List<Card> records)
    {
        var table = new ConsoleTable("Id", "Front-face", "Back-face");
        foreach (var item in records)
            table.AddRow(item.Id, item.Front, item.Back);
        Console.WriteLine("\t Cards-List");
        table.Write(ConsoleTables.Format.MarkDown);
    }
    public static void SessionStudyPrettyTable(List<StudySession> records)
    {
        var table = new ConsoleTable("Session Id", "User Name", "Time Init", "Time Fin", "Score","Stack Id");
        foreach (var item in records)
            table.AddRow(item.Id, item.User_, item.Init, item.Fin, item.Score, item.StackFk);
        Console.WriteLine("\t Study Sessions List");
        table.Write(ConsoleTables.Format.MarkDown);
    }
}

