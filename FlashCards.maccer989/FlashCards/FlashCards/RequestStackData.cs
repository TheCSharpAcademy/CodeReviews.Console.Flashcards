using CodingTrackerConsoleUI;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;

namespace FlashCardsUI
{
    public static class RequestStackData
    {
        public static void UpdateAStack(SqlStacksCrud sql)
        {
            Console.Clear();
            ReadAllStacks(sql);
            var stackId = GetNumberInput("\nPlease type the name of the Stack you want to update. Enter a valid number between 1 and 9 or type 0 to return to Main Menu");
            stackId = CheckValidRecord(sql, stackId);
            int stackName = GetNumberInput("\nPlease type the updated name of the Stack:");
            stackName = CheckDuplicateRecord(sql, stackName);
            DateTime lastUpdated = DateTime.Now;
            sql.UpdateStack(stackId, stackName);
        }
        public static void RemoveStack(SqlStacksCrud sql)
        {
            Console.Clear();
            ReadAllStacks(sql);
            var StackNameId = GetNumberInput("\nPlease type the name of the Stack you want to delete. Enter a valid number between 1 and 9 or type 0 to return to Main Menu\n\n");
            StackNameId = CheckValidRecord(sql, StackNameId);
            sql.RemoveStack(StackNameId);
        }
        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string stackNameUserInput = Console.ReadLine();
            if (stackNameUserInput == "0")
            {
                Console.WriteLine("Incorrect choice returning you to Main Menu");
                UserInterface.GetMainMenu();
            }
            int stackName = Validation.CheckValidNumber(stackNameUserInput);
            return stackName;            
        }
        public static void ReadAllStacks(SqlStacksCrud sql)
        {
            var rows = sql.GetAllStacks();
            TableVisualisation.ShowTable(rows);
        }
        public static void CreateNewStack(SqlStacksCrud sql)
        {
            string stackNameUserInput;
            int stackName;
            Console.WriteLine("Please enter new stack name use numbers 1 to 9 only:");
            stackNameUserInput = Console.ReadLine();
            stackName = Validation.CheckValidNumber(stackNameUserInput);         
            stackName = CheckDuplicateRecord(sql, stackName);
            StacksModel stack = new StacksModel
            {
                StackName = stackName
            };
            sql.CreateStack(stack);
        }        
        static int CheckValidRecord(SqlStacksCrud sql, int recordId)
        {
            bool recordExists = sql.CheckRecordExists(recordId);
            while (!recordExists)
            {
                recordId = GetNumberInput("\nStack name does not exist, please enter again. Enter a valid number between 1 and 9 or type 0 to return to Main Menu");
                recordExists = sql.CheckRecordExists(recordId);
            }
            return recordId;
        }
        static int CheckDuplicateRecord(SqlStacksCrud sql, int recordId)
        {
            bool recordExists = sql.CheckRecordExists(recordId);
            while (recordExists)
            {
                recordId = GetNumberInput("\nStack name exists, please enter again. Enter a valid number between 1 and 9 or type 0 to return to Main Menu");
                recordExists = sql.CheckRecordExists(recordId);
            }
            return recordId;
        }
    }
}
