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

        public bool AddStack(Stack_DTO dtoStack)
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

        public bool UpdateStack(Stack_DTO dtoStack)
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

        public bool DeleteStack(Stack_DTO dtoStack)
        {
            Stack newStack = new Stack() { Id = dtoStack.Id, Subject = dtoStack.Subject };
            int rows = _stacksDatabase.DeleteExistingStack(newStack);
            return rows > 0 ? true : false;
        }

        public List<Stack_DTO> GetAllStacks()
        {
            List<Stack_DTO> viewStacks = new List<Stack_DTO>();
            List<Stack> stacks = _stacksDatabase.GetStackResults("SELECT * FROM dbo.stacks");
            for (int i = 0; i < stacks.Count; i++) 
            {
                viewStacks.Add(new Stack_DTO() { DisplayId = i + 1, Id = stacks[i].Id, Subject = stacks[i].Subject });
            }
            return viewStacks;
        }

        public Stack_DTO GetStack(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", id);
            List<Stack> stacks = _stacksDatabase.GetStackResults("SELECT * FROM dbo.stacks WHERE Id = @Id", parameters);
            Stack_DTO stack = new Stack_DTO() { DisplayId = 1, Id = stacks[0].Id, Subject = stacks[0].Subject };
            return stack;
        }
    }
}
