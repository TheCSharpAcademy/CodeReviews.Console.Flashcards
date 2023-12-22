using System.Globalization;

namespace Flashcards;

class StudySession(string stackName, DateTime date, double score, int studySessionID = 0)
{
    public string StackName = stackName;
    public DateTime Date = date;
    public double Score = score;
    public int StudySessionID = studySessionID;

    public static StudySession FromCSV(string studySessionLine)
    {
        string[] data = studySessionLine.Split(',');
        DateTime.TryParseExact(data[1],"yyyy/MM/dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime date);
        StudySession studySession = new(data[0], date, Convert.ToDouble(data[2]));
        return studySession;
    }
}