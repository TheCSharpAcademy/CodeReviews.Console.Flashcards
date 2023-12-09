using System.Text.RegularExpressions;

namespace Flashcards;

class InputValidation
{
    public static string regexPattern = "(^$)|([^A-Za-z0-9$])";
    public static string? ValidateNewStackName(string? newStackName)
    {
        string? errorMessage = null;
        Regex stackNameRegex = new(regexPattern);
        string nonNullableNewStackName = newStackName ?? " ";

        if (nonNullableNewStackName.Length>50)
        {
            errorMessage += "Input has more than 50 characters. ";
        }

        if(stackNameRegex.IsMatch(nonNullableNewStackName))
        {
            errorMessage += "Input contains an invalid character. ";
        }

        return errorMessage;
    }

    public static string? ValidateStackSelection(List<Stacks> stacks, string? selection)
    {
        string? errorMessage = "The stack you selected does not exists. ";
        if(stacks.Any(stacks => stacks.StackName == selection))
        {
            errorMessage = null;
        }
        return errorMessage;
    }
}