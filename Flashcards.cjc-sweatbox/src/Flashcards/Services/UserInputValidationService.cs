using System.Globalization;
using Flashcards.Models;

namespace Flashcards.Services;

/// <summary>
/// Service to handle all user input validation.
/// </summary>
public static class UserInputValidationService
{
    #region Methods

    /// <summary>
    /// Performs validation on a Date user input for a Report.
    /// </summary>
    /// <param name="input">The Date user input.</param>
    /// <param name="format">The format string that the date input should match.</param>
    /// <returns>The validation result and message.</returns>
    public static UserInputValidationResult IsValidReportDate(string input, string format)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new UserInputValidationResult(false, "Report date cannot be empty.");
        }

        bool isCorrectDateFormat = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        if (!isCorrectDateFormat)
        {
            return new UserInputValidationResult(false, $"Report date in wrong format. Format = {format}.");
        }

        return new UserInputValidationResult(true, "Validation successful");
    }

    #endregion
}
