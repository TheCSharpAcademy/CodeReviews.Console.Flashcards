using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Lonchanick9427.FlashCard.ExcutionFolder;

public static class SessionOperations
{
    public static void New()
    {
        Console.Clear();
        Console.WriteLine("Pick up an Stack to Start a new Study Session (Trype ! to exit)");
        List<int> i = StackOperations.GetStackIdList(StackOperations.showStacks());
        //Console.WriteLine("Pick any Stack (Id) From the list");
        int stackId = ToolBox.GetIntInput("Stack Id");
        while (!(i.IndexOf(stackId) >= 0))
        {
            Console.WriteLine("The Stack Id provided does not exist! ");
            stackId = ToolBox.GetIntInput("Stack-Id");
        }
        StudySession session = new();
        session.User_ = ToolBox.GetStringInput("Enter ur User Name");
        
        
        session.Init = DateTime.Now;
        Console.WriteLine($"Study Session starts at {session.Init}");

        List<Card> cards = DB.CardDB.CardsByStackId(stackId);

        foreach(var x in cards)
        {
            Console.WriteLine("\t"+x.Front);
            Console.ReadLine();
            Console.WriteLine("\t" + x.Back);
        }

        session.Fin = DateTime.Now;
        session.Score = 31415;
        session.StackFk = stackId;
        DB.StudySessionDB.Add(session);
        Console.WriteLine($"Study Session Ends at {session.Fin}");
        Console.ReadLine();
    }

    public static void ShowAll()
    {
        Console.Clear();
        var aux = DB.StudySessionDB.ShowAll();
        ToolBox.SessionStudyPrettyTable(aux);
        Console.ReadLine();
    }


}
