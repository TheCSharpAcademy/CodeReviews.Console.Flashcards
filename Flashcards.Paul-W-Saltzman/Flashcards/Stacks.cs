
namespace Flashcards
{
    internal class Stack
    {
        internal int StackID {get;set;}
        internal string StackName { get;set;}

        internal Stack() {}

        internal Stack(string stackName)
        {
            StackName = stackName;
            StackID = Data.EnterStack(StackName);
        }

        internal static void LoadSeedDataStacks()
        {
            Data.EnterStack("Variable Types");
            Data.EnterStack("Selector Codes");
            Data.EnterStack("French");
            Data.EnterStack("Vietnamese");
            Data.EnterStack("Spanish");

        }
    }
}
