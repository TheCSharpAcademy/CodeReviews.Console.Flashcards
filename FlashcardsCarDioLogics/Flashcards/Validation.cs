namespace Flashcards;

public class Validation
{
    public void IsStringValid(string stringInput, out bool validString)
    {
        if (string.IsNullOrEmpty(stringInput))
        {
            Console.WriteLine("Invalid input. The string cannot be null or empty.");
            validString = false;
        }
        else
        {
            validString = true;
        }
    }

    public int GetValidInt(string input, out bool validInt)
    {
        int validIntInput;
        if (int.TryParse(input, out validIntInput))
        {
            validInt = true;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter an integer.");
            validInt = false;
        }

        return validIntInput;
    }

    public void StackExistsCheck(string stringInput, List<string> stacksName, out bool stackExists)
    {
        stackExists = false;

        foreach (string stackName in stacksName)
        {
            if(stringInput == stackName)
            {
                stackExists = true;
            }
        }
    }
}
