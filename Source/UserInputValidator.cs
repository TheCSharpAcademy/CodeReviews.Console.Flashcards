using System.Globalization;
using Dapper;
using Spectre.Console;

namespace vcesario.Flashcards;

public class UserInputValidator
{
    public ValidationResult ValidateDateTimeOrReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_DATETIMEERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidateDateOrReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!DateOnly.TryParseExact(input, "dd/MM/yyyy", out DateOnly result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_DATEERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidateLongReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!long.TryParse(input, out long result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_LONGERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidatePositiveIntOrReturn(string input)
    {
        if (input.ToLower().Equals("return"))
        {
            return ValidationResult.Success();
        }

        if (!uint.TryParse(input, out uint result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_LONGERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ValidatePositiveIntOrPeriod(string input)
    {
        if (input.ToLower().Equals("."))
        {
            return ValidationResult.Success();
        }

        if (!uint.TryParse(input, out uint result))
        {
            return ValidationResult.Error(ApplicationTexts.USERINPUT_LONGERROR);
        }

        return ValidationResult.Success();
    }

    public ValidationResult ConfirmUniqueStackName(string input)
    {
        using (var connection = DataService.OpenConnection())
        {
            string sql = "SELECT Id FROM Stacks WHERE Name=@Name";
            var result = connection.QueryFirstOrDefault(sql, new { Name = input });

            if (result != null)
            {
                return ValidationResult.Error(ApplicationTexts.USERINPUT_EXISTINGSTACK);
            }
        }
        return ValidationResult.Success();
    }
}