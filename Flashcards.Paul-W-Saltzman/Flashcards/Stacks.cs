
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
            Stack variableTypes = new Stack("Variable Types");
            Stack selectorCodes = new Stack("Selector Codes");
            Stack french = new Stack("French");
            Stack vietnamese = new Stack("Vietnamese");
            Stack Spanish = new Stack("Spanish");

        }
    }
}
