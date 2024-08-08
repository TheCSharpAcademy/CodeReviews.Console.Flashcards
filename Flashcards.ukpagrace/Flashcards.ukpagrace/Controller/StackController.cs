using Flashcards.ukpagrace.Database;
using Flashcards.ukpagrace.Utility;
using Spectre.Console;

namespace Flashcards.ukpagrace.Controller

{
    class StackController
    {
        FlashCardDatabase flashCardDatabase = new();
        UserInput userInput = new ();
        StackDatabase stackDatabase = new();

        public void CreateTable()
        {
            stackDatabase.CreateStack();
        }
        public void CreateStack()
        {
            var stackName = userInput.GetStackInput();
            stackDatabase.Insert(stackName);
        }

        public void UpdateStack()
        {
            string stackName = userInput.GetStackOption();
            int stackId = stackDatabase.GetStackId(stackName);

            string stackInput = userInput.GetStackInput();
            
            stackDatabase.Update(stackId, stackInput);
        }

        public void DeleteStack()
        {
            string stackName = userInput.GetStackOption();
            int stackId = stackDatabase.GetStackId(stackName);
            if (!stackDatabase.IdExists(stackId))
            {
                AnsiConsole.MarkupLine("[red]Id does not exists[/]");
                return;
            }
            
            stackDatabase.Delete(stackId);
        }
    }
}