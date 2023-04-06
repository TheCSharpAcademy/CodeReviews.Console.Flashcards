using ConsoleTableExt;
using FlashCardsLibrary;
using sadklouds.FlashCards.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ConsoleTableBuilder
               .From(stacks)
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
