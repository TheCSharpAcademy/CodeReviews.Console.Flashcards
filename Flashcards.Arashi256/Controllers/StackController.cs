using Dapper;
using Flashcards.Arashi256.Classes;
using Flashcards.Arashi256.Models;

namespace Flashcards.Arashi256.Controllers
{
    internal class StackController
    {
        private StacksDatabase _stacksDatabase;

        public StackController()
        {
            _stacksDatabase = new StacksDatabase();
        }

        public bool AddStack(StackDto dtoStack)
        {
            if (_stacksDatabase.CheckDuplicateStack(dtoStack.Subject))
            {
                Stack newStack = new Stack() { Id = dtoStack.Id, Subject = dtoStack.Subject };
                int rows = _stacksDatabase.AddNewStack(newStack);
                return rows > 0 ? true : false;
            }
            else
                return false;
        }

        public bool UpdateStack(StackDto dtoStack)
        {
            if (_stacksDatabase.CheckDuplicateStack(dtoStack.Subject))
            {
                Stack newStack = new Stack() { Id = dtoStack.Id, Subject = dtoStack.Subject };
                int rows = _stacksDatabase.UpdateExistingStack(newStack);
                return rows > 0 ? true : false;
            }
            else
                return false;
        }

        public bool DeleteStack(StackDto dtoStack)
        {
            Stack newStack = new Stack() { Id = dtoStack.Id, Subject = dtoStack.Subject };
            int rows = _stacksDatabase.DeleteExistingStack(newStack);
            return rows > 0 ? true : false;
        }

        public List<StackDto> GetAllStacks()
        {
            List<StackDto> viewStacks = new List<StackDto>();
            List<Stack> stacks = _stacksDatabase.GetStackResults("SELECT * FROM dbo.stacks");
            for (int i = 0; i < stacks.Count; i++) 
            {
                viewStacks.Add(new StackDto() { DisplayId = i + 1, Id = stacks[i].Id, Subject = stacks[i].Subject });
            }
            return viewStacks;
        }

        public StackDto GetStack(int id)
        {
            StackDto stack;
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            List<Stack> stacks = _stacksDatabase.GetStackResults("SELECT * FROM dbo.stacks WHERE Id = @Id", parameters);
            stack = new StackDto() { DisplayId = 1, Id = stacks[0].Id, Subject = stacks[0].Subject };
            return stack;
        }
    }
}