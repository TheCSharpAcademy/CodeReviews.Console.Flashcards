using Spectre.Console;

namespace Flash.Helper.StudyHelper;
internal class SetUpStudyStack
{
    internal static int GetSetUpStudyStack()
    {
        AnsiConsole.Markup("Insert the [red]Stack_Priamry_Id[/] of the stack you want to study\n");
        

        string studyStackString = Console.ReadLine();
        int studyStack;

        if (int.TryParse(studyStackString, out studyStack))
        {
            Console.WriteLine($"Selected Stack_Primary_Id: {studyStack}\n");
        }
        else
        {
            Console.WriteLine("something's wrong\n");
        }
        return studyStack;
    }
}
