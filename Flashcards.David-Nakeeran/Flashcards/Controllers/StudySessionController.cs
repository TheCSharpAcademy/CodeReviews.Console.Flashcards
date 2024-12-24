using Flashcards.Utilities;
using Spectre.Console;

class StudySessionController
{
    private readonly Validation _validation;
    private readonly Conversion _conversion;

    public StudySessionController(Validation validation, Conversion conversion)
    {
        _validation = validation;
        _conversion = conversion;
    }

    internal int GetStudySessionYear()
    {
        var yearString = AnsiConsole.Ask<string>("Please enter year you'd like to view for study session summary.");
        yearString = _validation.CheckInputNullOrWhitespace("Please enter year you'd like to view for study session summary.", yearString);
        int year = _conversion.ParseInt(yearString, "Please make sure to enter year with numerical values.");
        return year;
    }

}