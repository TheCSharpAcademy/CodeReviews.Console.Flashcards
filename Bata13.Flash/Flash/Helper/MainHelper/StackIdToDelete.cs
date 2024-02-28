namespace Flash.Helper.MainHelper;
internal class StackIdToDelete
{
    internal static int GetStackIdToDelete()
    {
        Console.WriteLine("Enter the Stack_Primary_Id to delete:");
        string stackToDelete = Console.ReadLine();
        int stackIdToDelete = 0;
        try
        {
            int.TryParse(stackToDelete, out stackIdToDelete);
            Console.WriteLine($"Selected Stack_Primary_Id: {stackIdToDelete}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            Console.WriteLine("Unable to convert the string to an integer.");
        }
        return stackIdToDelete;
    }
}