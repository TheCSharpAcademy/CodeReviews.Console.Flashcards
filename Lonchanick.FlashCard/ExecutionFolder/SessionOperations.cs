namespace Lonchanick9427.FlashCard.ExcutionFolder;

public static class SessionOperations
{
    public static void New()
    {
        Console.Clear();
        Console.WriteLine("Pick up an Stack to Start a new Study Session (Trype ! to exit)");
        List<int> i = StackOperations.GetStackIdList(StackOperations.showStacks());
        int stackId = ToolBox.GetIntInput("Stack Id");
        while (!(i.IndexOf(stackId) >= 0))
        {
            Console.WriteLine("The Stack Id provided does not exist! ");
            stackId = ToolBox.GetIntInput("Stack-Id");
        }
        StudySession session = new();
        session.User_ = ToolBox.GetStringInput("Enter ur User Name");
        
        Console.Clear() ;
        

        session.Init = DateTime.Now;
        
        List<Card> cards = DB.CardDB.CardsByStackId(stackId);
        int size = cards.Count();
        if (size > 0 )
        {
            string back;
            string backCard;
            int score=0;
            foreach (var x in cards)
            {
                Console.WriteLine($"STUDY SESSION");
                Console.WriteLine($"User: {session.User_}\n\n");
                Console.WriteLine($"Score: {score}/{size}\n\n");
                Console.WriteLine("\tFront: " + x.Front);
                back = ToolBox.GetStringInput("\tBack");
                
                backCard = x.Back.ToLower();
                back = back.ToLower();
                if (backCard == back)
                {
                    Console.WriteLine("Right!");
                    score++;
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Wrong!");
                    Console.ReadLine();
                }
                Console.Clear();
            }
            session.Fin = DateTime.Now;
            session.Score = score;
            session.StackFk = stackId;
            DB.StudySessionDB.Add(session);
            Console.WriteLine($"\tStudy Session Ended");
            Console.WriteLine($"Results");
            session.Print();
            Console.ReadLine();
        }
    }

    public static void ShowAll()
    {
        Console.Clear();
        var aux = DB.StudySessionDB.ShowAll();
        ToolBox.SessionStudyPrettyTable(aux);
        Console.ReadLine();
    }
    public static List<int> GetStudySessionIdList(List<StudySession> param)
    {
        List<int> i = new();
        foreach (var x in param)
            i.Add(x.Id);

        return i;
    }
    public static void Delete()
    {
        Console.Clear();
        var aux = DB.StudySessionDB.ShowAll();
        ToolBox.SessionStudyPrettyTable(aux);
        var i = GetStudySessionIdList(aux);
        Console.WriteLine("Pick any Study Session from the list");
        int sessionId = ToolBox.GetIntInput("Study Session Id");

        while (!(i.IndexOf(sessionId) >= 0))
        {
            Console.WriteLine("The Study Session Id provided does not exist! ");
            sessionId = ToolBox.GetIntInput("Study Session-Id");
        }
        try
        { 
            DB.StudySessionDB.Delete(sessionId);
            Console.WriteLine("Done!");Console.ReadLine();
        }catch
        {
            Console.WriteLine("Error de algun tipo");
        }
    }

}
