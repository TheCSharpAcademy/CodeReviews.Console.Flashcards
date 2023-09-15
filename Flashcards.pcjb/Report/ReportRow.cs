namespace Flashcards;

class ReportRow
{
    public string StackName { get; set; }
    public int January { get; set; }
    public int February { get; set; }
    public int March { get; set; }
    public int April { get; set; }
    public int May { get; set; }
    public int June { get; set; }
    public int July { get; set; }
    public int August { get; set; }
    public int September { get; set; }
    public int October { get; set; }
    public int November { get; set; }
    public int December { get; set; }

    public ReportRow(string stackName, int[] monthlyValues)
    {
        StackName = stackName;
        January = monthlyValues[0];
        February = monthlyValues[1];
        March = monthlyValues[2];
        April = monthlyValues[3];
        May = monthlyValues[4];
        June = monthlyValues[5];
        July = monthlyValues[6];
        August = monthlyValues[7];
        September = monthlyValues[8];
        October = monthlyValues[9];
        November = monthlyValues[10];
        December = monthlyValues[11];
    }
}