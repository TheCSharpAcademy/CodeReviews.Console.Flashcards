namespace Flash.Helper.SubManageStacksHelper.ViewXAmountFlashcardHelper;
internal class SelectXAmount
{
    internal static int GetSelectXAmount(string xAmountString)
    {
        int xAmount;

        if (int.TryParse(xAmountString, out xAmount))
        {
            // Conversion successful, xAmount now contains the integer value
            Console.WriteLine("xAmount is: " + xAmount);
        }
        else
        {
            // Conversion failed, handle the invalid input here
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }

        return xAmount;
    }

}
