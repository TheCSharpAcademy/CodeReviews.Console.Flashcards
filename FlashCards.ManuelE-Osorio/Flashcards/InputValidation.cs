using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Flashcards;

class InputValidation
{
    public static bool ValidateNewStack(string? newStackName)
    {
        bool newStackIsValid = false;
        Regex r = new("(^$)|([^A-Za-z0-9$])");
        string nonNullableNewStackName = newStackName ?? " ";
        if (nonNullableNewStackName.Length<=50 && !r.IsMatch(nonNullableNewStackName))
        {
            newStackIsValid = true;
        }
        return newStackIsValid;
    }
}