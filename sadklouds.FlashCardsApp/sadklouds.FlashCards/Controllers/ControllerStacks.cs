using ConsoleTableExt;
using FlashCardsLibrary;
using FlashCardsLibrary.Tools;
using sadklouds.FlashCards.Helpers;

namespace sadklouds.FlashCards.Controllers
{
    internal class ControllerStacks
    {
        private readonly SQLDataAccess db = new();

        public void GetStacks()
        {
            var stacks = db.LoadStacks();
            bool popluatedCheck = stacks.Count == 0;
            if (popluatedCheck == false)
            {
                var stackDTO = CreateDTOHelper.CreateStackDTO(stacks);
                ConsoleTableBuilder
               .From(stackDTO)
               .ExportAndWriteLine();
            }
            else
            {
                Console.WriteLine("No records found");
            }
        }

        public bool checkStackName(string stackName)
        {
            bool output = db.CheckStackName(stackName);
            return output;
        }

        public void AddStack()
        {
            string stackName = UserInputHelper.GetUserStringInput("\nEnter stack name for new stack: ");
            if (checkStackName(stackName) == false)
                db.CreateStack(stackName);
            else
                Console.WriteLine($"\nStack '{stackName}' already exists!\n");
        }

        public void RemoveStack(string stackName)
        {
            db.DeleteStack(stackName);
        }
    }
}
