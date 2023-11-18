using System.Data;

namespace Flashcards
{
    internal class StudySessionReport
    {
        internal int StackID;
        internal String StackName;
        internal int YEAR;
        internal int January;
        internal int February;
        internal int March;
        internal int April;
        internal int May;
        internal int June;
        internal int July;
        internal int August;
        internal int September;
        internal int October;
        internal int November;
        internal int December;


        internal static void ShowReport(List<StudySessionReport> studySessions)
        {


            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Stack", typeof(string));
            dataTable.Columns.Add("Year", typeof(int));
            dataTable.Columns.Add("JAN", typeof(int));
            dataTable.Columns.Add("FEB", typeof(int));
            dataTable.Columns.Add("MAR", typeof(int));
            dataTable.Columns.Add("APR", typeof(int));
            dataTable.Columns.Add("MAY", typeof(int));
            dataTable.Columns.Add("JUN", typeof(int));
            dataTable.Columns.Add("JUL", typeof(int));
            dataTable.Columns.Add("AUG", typeof(int));
            dataTable.Columns.Add("SEP", typeof(int));
            dataTable.Columns.Add("OCT", typeof(int));
            dataTable.Columns.Add("NOV", typeof(int));
            dataTable.Columns.Add("DEC", typeof(int));

            foreach (StudySessionReport report in studySessions) 
            {
                dataTable.Rows.Add(report.StackName,report.YEAR,report.January,report.February,report.March,report.April,report.May,report.June,report.July,report.August,report.September,report.October,report.November,report.December);
            }

            Helpers.ShowTable(dataTable, "Coding Sessions for the Year");
            Console.ReadKey();


        }
    }
}
