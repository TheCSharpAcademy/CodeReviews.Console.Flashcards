using Flashcards.Object_Classes;
using Flashcards.Validation;

namespace Flashcards.Database
{
    public static class StackManager
    {
        public static void CreateStack()
        {
            Console.WriteLine("Please provide a name for your new Flashcard Stack.");
            string? stackName = Console.ReadLine();
            while(InputValidation.IsValidString(stackName) == false)
            {
                Console.WriteLine("Please provide a name for your new Flashcard Stack.");
                stackName = Console.ReadLine();
            }

            DatabaseManager.CreateStack(stackName);
            Console.WriteLine($"\nStack with name {stackName} successfully created.");
        }

        public static void UpdateStack()
        {
            if (!DatabaseManager.DoWeHaveFlashcardStacks())
            {
                Console.WriteLine("No flashcard stacks found. Please create a stack first.");
                return;
            }
            ViewAllFlashcardStacks();

            int stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you wish to update, or type 0 to return to the Main Menu.");

            while (stackID != 0 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("StackTable"),
                "StackID", stackID) == false)
            {
                Console.WriteLine("Please provide a valid Stack ID.");
                stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you wish to update, or type 0 to return to the Main Menu.");
            }

            Console.WriteLine("Please provide a name for your new Flashcard Stack.");
            string? stackName = Console.ReadLine();
            while (InputValidation.IsValidString(stackName) == false)
            {
                Console.WriteLine("Please provide a name for your new Flashcard Stack.");
                stackName = Console.ReadLine();
            }

            DatabaseManager.UpdateStack(stackName, stackID);
            Console.WriteLine($"\nStack with Stack ID {stackID} successfully updated.");
        }

        public static void DeleteStack()
        {
            if (!DatabaseManager.DoWeHaveFlashcardStacks())
            {
                Console.WriteLine("No flashcard stacks found. Please create a stack first.");
                return;
            }
            ViewAllFlashcardStacks();

            int stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you wish to delete, or type 0 to return to the Main Menu.");

            while (stackID != 0 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("StackTable"),
                "StackID", stackID) == false)
            {
                Console.WriteLine("Please provide a valid Stack ID.");
                stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you wish to delete, or type 0 to return to the Main Menu.");
            }

            DatabaseManager.DeleteStack(stackID);
            Console.WriteLine($"\nStack with Stack ID {stackID} successfully deleted.");
        }

        public static void ViewFlashcardStacks()
        {
            if (!DatabaseManager.DoWeHaveFlashcardStacks())
            {
                Console.WriteLine("No flashcard stacks found. Please create a stack first.");
                return;
            }
            int stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you wish to view, type -1 to view all stacks \n or type 0 to return to the Main Menu.");
            while (stackID != 0 && stackID != -1 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("StackTable"),
                "StackID", stackID) == false)
            {
                Console.WriteLine("Please provide a valid Stack ID.");
                stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you wish to view, type -1 to view all stacks \n or type 0 to return to the Main Menu.");
            }
            List<Stack> stacks = DatabaseManager.GetFlashcardStacks(stackID);
            Console.WriteLine("----------------------------------------------\n");
            foreach (var stack in stacks)
            {
                Console.WriteLine("\nFlashcard Stack");
                Console.WriteLine($"ID: {stack.StackId} | Stack Name: {stack.StackName}");
                Console.WriteLine("----------------------------------------------\n");
                if(DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("FlashcardTable"),"StackID", stack.StackId) == true) 
                {
                    List<FlashcardDTO> flashcards = DatabaseManager.GetFlashcards(stack.StackId);
                    Console.WriteLine("Flashcards\n");
                    int index = 1; //We want them to be in order
                    foreach (var card in flashcards)
                    {
                        Console.WriteLine($"ID: {index} | Question: {card.Question} | Answer: {card.Answer}");
                        Console.WriteLine("----------------------------------------------");
                        index++;
                    }                   
                }
                else
                {
                    Console.WriteLine($"No Flashcards for stack {stack.StackName}");
                }
                
            }
        }

        public static void ViewAllFlashcardStacks()
        {
            List<Stack> stacks = DatabaseManager.GetFlashcardStacks();
            Console.WriteLine("----------------------------------------------\n");
            foreach (var stack in stacks)
            {
                Console.WriteLine($"ID: {stack.StackId} | Stack Name: {stack.StackName}");
            }
            Console.WriteLine("----------------------------------------------\n");
        }
    }
}
