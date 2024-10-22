namespace Flashcards.harris_andy.Classes;

public class StudyReport
{
    public string StackName { get; set; } = string.Empty;
    public object January { get; set; } = 0;
    public object February { get; set; } = 0;
    public object March { get; set; } = 0;
    public object April { get; set; } = 0;
    public object May { get; set; } = 0;
    public object June { get; set; } = 0;
    public object July { get; set; } = 0;
    public object August { get; set; } = 0;
    public object September { get; set; } = 0;
    public object October { get; set; } = 0;
    public object November { get; set; } = 0;
    public object December { get; set; } = 0;

    public StudyReport() { }

    public StudyReport(
        string stackName,
        object january,
        object february,
        object march,
        object april,
        object may,
        object june,
        object july,
        object august,
        object september,
        object october,
        object november,
        object december)
    {
        StackName = stackName;
        January = january;
        February = february;
        March = march;
        April = april;
        May = may;
        June = june;
        July = july;
        August = august;
        September = september;
        October = october;
        November = november;
        December = december;
    }
}