using FlashCards.Database;
using FlashCards.Models;

namespace FlashCards.Control
{
    internal static class StackControl
    {
        internal static List<StackWithCleanId> LaunderStackId(List<Stack> stacks)
        {
            int cleanID = 1;
            List<StackWithCleanId> cleanStacks = new List<StackWithCleanId>();

            foreach (Stack stack in stacks)
            {
                StackWithCleanId cleanStack = new StackWithCleanId
                {
                    CleanId = cleanID,
                    Id = stack.Id,
                    Name = stack.Name
                };
                cleanStacks.Add(cleanStack);
                cleanID++;
            }
            return cleanStacks;
        }

        internal static void ViewStacks(List<StackWithCleanId> stacks)
        {
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Existing FlashCard Stacks:");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("********************************************************************************");
            foreach (StackWithCleanId stack in stacks)
            {
                Console.WriteLine($"FlashCard Stack ID: {stack.CleanId} --- FlashCard Stack Name: {stack.Name}");
            }
            Console.WriteLine("********************************************************************************\n");
        }

        internal static void CreateStack()
        {
            Console.WriteLine("Enter the name of the FlashCard Stack, or enter 0 to return.");
            string stackName = EnforceUniqueStackName();

            if (stackName == "0")
            {
                Console.Clear();
                return;
            }

            Stack stack = new Stack { Name = stackName };
            StackDBOperations.AddStack(stack);
            return;
        }

        internal static void DeleteStack()
        {
            List<Stack> stacks = StackDBOperations.GetStacks();
            List<StackWithCleanId> cleanStacks = LaunderStackId(stacks);
            ViewStacks(cleanStacks);

            while (true)
            {
                Console.WriteLine("Enter the Id of the FlashCard Stack to delete, or enter 0 to return.");

                string stackId = Helper.GetIDInput();

                if (stackId == "0")
                {
                    Console.Clear();
                    return;
                }

                int id = int.Parse(stackId);

                foreach (StackWithCleanId stack in cleanStacks)
                {
                    if (stack.CleanId == id)
                    {
                        StackDBOperations.DeleteStack(stack.Id);
                        Console.Clear();
                        return;
                    }
                }

                Console.WriteLine("Invalid ID. Please try again.\n");
            }
        }

        internal static void RenameStack()
        {
            List<Stack> stacks = StackDBOperations.GetStacks();
            List<StackWithCleanId> cleanStacks = LaunderStackId(stacks);
            ViewStacks(cleanStacks);
            int id = 0;

            while (true)
            {
                bool validID = false;
                int cleanID = 0;
                while (!validID)
                {
                    Console.WriteLine("Enter the Id of the FlashCard Stack to update, or enter 0 to return.");

                    string stackID = Helper.GetIDInput();
                    if (stackID == "0")
                    {
                        Console.Clear();
                        return;
                    }

                    cleanID = int.Parse(stackID);

                    foreach (StackWithCleanId stack in cleanStacks)
                    {
                        if (stack.CleanId == cleanID)
                        {
                            id = stack.Id;
                            validID = true;
                        }
                    }

                    if (!validID)
                    {
                        Console.WriteLine("Invalid ID. Please try again.\n");
                    }
                }

                Console.WriteLine("Enter the new name, or enter 0 to return.");
                string newName = EnforceUniqueStackName();

                if (newName == "0")
                {
                    Console.Clear();
                    return;
                }

                StackDBOperations.RenameStack(id, newName);
                Console.Clear();
                return;
            }
        }

        internal static string EnforceUniqueStackName()
        {
            List<Stack> stacks = StackDBOperations.GetStacks();
            string stackName = null;
            bool nameOK = false;

            while (!nameOK)
            {
                bool duplicate = false;
                stackName = Helper.GetStringInput();
                foreach (Stack stack in stacks)
                {
                    if (stack.Name == stackName)
                    {
                        Console.WriteLine("A FlashCard Stack with that name already exists. Please enter a unique name.");
                        duplicate = true;
                    }
                }

                if (!duplicate)
                {
                    nameOK = true;
                }
            }
            return stackName;
        }
    }
}
