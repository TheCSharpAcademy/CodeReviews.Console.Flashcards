using Dapper;
using Flashcards.Study.Models;
using Flashcards.Study.Models.Domain;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System.Configuration;
namespace Flashcards.Study
{
    public class DatabaseAccess
    {
        private readonly string _ConnectionString = ConfigurationManager.ConnectionStrings["defaultconnection"].ConnectionString;
        public static string ConnectionString { get; set; }

        public DatabaseAccess()
        {
            ConnectionString = _ConnectionString;
        }

        public static void CreateDbIfNotExists()
        {
            try
            {
                string query = @"If DB_ID('stacksNFlashcards') is null
                              Begin
                               create DataBase stacksNFlashcards
                              END;";
                using SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                int row = conn.Execute(query);
                if (row > 0)
                {
                    Console.WriteLine("The dataBase stacksNFlashcards is created for you");
                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured while connecting with DB " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("general exception " + ex.Message);
            }
            ConnectionString = ConnectionString + "; Initial Catalog= stacksNFlashcards;";
        }

        public static async Task CreateTables()
        {
            string DBConnectionString = ConnectionString;
            string stacksquery = @"if object_id('stacks') is null
                                 BEGIN
                                    Create Table stacks(
                                      stackname varchar(20) primary key
                                    );                                    
                                  END";
            string Flashcardsquery = @"if object_id('Flashcards') is null
                                    BEGIN 
                                        Create Table Flashcards(
                                              ID int primary key identity(1,1),
                                              Front varchar(100),
                                              Back Varchar(100),
                                              stack_name varchar(20),
                                              Foreign key(stack_name) references stacks(stackname) ON DELETE CASCADE);
                                    END";
            string studysession = @"if object_id('studysessions') is null
                                 BEGIN
                                      create table studysessions(
                                               SessionId int PRIMARY key identity(1,1),
                                               StackName varchar(20),
                                               score float,
                                               DateOfSession date Default convert(date,GetDate())
                                                               );
                                 END";
            try
            {
                SqlConnection conn = new SqlConnection(DBConnectionString);
                conn.Open();
                conn.Execute(stacksquery);
                conn.Execute(Flashcardsquery);
                conn.Execute(studysession);
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occured while connecting with DB" + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Console.ReadLine();
        }
        public static List<string> GetAllStacks()
        {
            List<string> results = new List<string>();
            try
            {
                string query = @"Select * from stacks";
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                results = (conn.Query<string>(query)).ToList();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("an error connecting to stacks table: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return results;
        }
        public static void InsertNewStack(string newstackname)
        {
            try
            {
                string query = "insert into stacks([stackname]) values (@stackname)";
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                conn.Execute(query, new { stackname = newstackname });
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An Error occured while connecting to DB " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void DeleteStack(string stackname)
        {
            try
            {
                string query = "Delete from stacks where stackname=@name";
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                conn.Execute(query, new { name = stackname });
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An Error occured while connecting to DB " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static List<FlashcardDto> GetAllFlashcards()
        {
            List<FlashcardDto> FlashcardDTOs = new List<FlashcardDto>();
            try
            {
                string query = @"select * from Flashcards";
                SqlConnection conn = new SqlConnection(ConnectionString);
                using (conn)
                {
                    conn.Open();
                    List<Flashcard> Flashcards = conn.Query<Flashcard>(query).ToList();
                    conn.Close();
                    int count = 1;
                    foreach (Flashcard fl in Flashcards)
                    {
                        FlashcardDTOs.Add(new FlashcardDto
                        {
                            ID = count,
                            Front = fl.Front,
                            Back = fl.Back,
                        });
                        count++;
                    }
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return FlashcardDTOs;
        }
        public static void DeleteFlashcard(int id)
        {
            List<Flashcard> Flashcards;
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                using (conn)
                {
                    conn.Open();
                    Flashcards = conn.Query<Flashcard>("select * from Flashcards").ToList();
                    conn.Execute("delete from Flashcards where ID=@id", new { id = Flashcards[id - 1].ID });
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static List<FlashcardDto> GetAllFlashcardsofStack(string stackpicked)
        {
            List<FlashcardDto> Flashcards = null;
            string query = "select * from Flashcards where stack_name=@stackname";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    int count = 1;
                    Flashcards = conn.Query<Flashcard>(query, new { stackname = stackpicked }).Select(x => new FlashcardDto
                    {
                        ID = count++,
                        Front = x.Front,
                        Back = x.Back
                    }).ToList();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Flashcards;
        }
        public static void CreateNewFlashcardforstack(string stackpicked)
        {
            FlashcardDto newFlashcard = UserInputs.CreateNewFlashcard();
            string query = "insert into Flashcards([Front],[Back],[stack_name]) values (@front,@back,@stackname)";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    conn.Execute(query, new { front = newFlashcard.Front, back = newFlashcard.Back, stackname = stackpicked });
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void DeleteFlashcardforstack(string stackpicked, List<FlashcardDto> FlashcardsDTO)
        {
            int userChoice = UserInputs.DeleteFlashcardforStack(FlashcardsDTO);
            string query = "select * from Flashcards where stack_name=@stackname";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                List<Flashcard> Flashcards = conn.Query<Flashcard>(query, new { stackname = stackpicked }).ToList();
                conn.Execute("delete from Flashcards where ID=@id", new { id = Flashcards[userChoice - 1].ID });
                conn.Close();
            }
        }
        public static void EditFlashcardforstack(string stackpicked, List<FlashcardDto> FlashcardsDTO)
        {
            int userChoice = UserInputs.EditFlashcardforStack(FlashcardsDTO);
            string query = "select * from Flashcards where stack_name=@stackname";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                List<Flashcard> Flashcards = conn.Query<Flashcard>(query, new { stackname = stackpicked }).ToList();
                var editedFlashcard = UserInputs.GeteditedFlashcardDto();
                string editQuery = "update Flashcards set Front=@front, Back=@back where ID=@id";
                conn.Execute(editQuery, new { front = editedFlashcard.Front, back = editedFlashcard.Back, id = Flashcards[userChoice - 1].ID });
            }
        }
        public static Dictionary<string, string> QAOfStacks(string stackpicked)
        {
            Dictionary<string, string> QnA = new Dictionary<string, string>();
            List<FlashcardDto> cards = GetAllFlashcardsofStack(stackpicked);
            cards.ForEach(x => QnA.Add(x.Front, x.Back));
            return QnA;
        }
        public static void LogStudySession(double userscore, string stackpicked)
        {
            string insertQuery = @"INSERT INTO studysessions([StackName],[score]) Values 
                                  (@stackname,@score)";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute(insertQuery, new { stackname = stackpicked, score = userscore });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void GetAllReport()
        {
            Dictionary<string, List<FinalReport>> reportdictionary = new Dictionary<string, List<FinalReport>>();
            List<FinalReport> finalreportrows;
            string pivotQuery = @"with yeartable as(
select StackName, year(DateOfSession) as sessionyear ,month(DateOfSession) as sessionmonth, avg(score) as averagescore from studysessions 
group by DateOfSession, StackName)

select stackname, sessionyear, 
isnull([1] ,0) as january,
ISNULL([2] ,0) as February,
ISNULL([3] ,0) as March,
ISNULL([4] ,0) as April,
ISNULL([5] ,0) as May,
ISNULL([6] ,0) as June,
ISNULL([7] ,0) as July,
ISNULL([8] ,0) as August,
ISNULL([9] ,0) as September,
ISNULL([10] ,0) as October,
ISNULL([11] ,0) as November,
ISNULL([12] ,0) as December
from(
select * from yeartable) as sourcedata
pivot(
      Max(averagescore)
	  for sessionmonth in ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
	  ) as pivotetable
order by  sessionyear desc, StackName;";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                finalreportrows = conn.Query<FinalReport>(pivotQuery).ToList();
            }
            foreach (var yearrow in finalreportrows)
            {
                if (reportdictionary.ContainsKey(yearrow.Sessionyear))
                {
                    reportdictionary[yearrow.Sessionyear].Add(yearrow);
                }
                else
                {
                    reportdictionary[yearrow.Sessionyear] = new List<FinalReport>();
                    reportdictionary[yearrow.Sessionyear].Add(yearrow);
                }
            }
            foreach (var kvp in reportdictionary)
            {
                Table yearwisereport = new Table();
                yearwisereport.Title = new TableTitle($"Consolidated report for {kvp.Key} year");
                var props = typeof(FinalReport).GetProperties();
                foreach (var prop in props)
                {
                    yearwisereport.AddColumn(prop.Name);
                }
                foreach (var yearwiseobjects in kvp.Value)
                {
                    var rowValues = new List<string> { yearwiseobjects.StackName };

                    // Add all month values formatted to 1 decimal place
                    rowValues.Add($"{yearwiseobjects.January:0.0}");
                    rowValues.Add($"{yearwiseobjects.February:0.0}");
                    rowValues.Add($"{yearwiseobjects.March:0.0}");
                    rowValues.Add($"{yearwiseobjects.April:0.0}");
                    rowValues.Add($"{yearwiseobjects.May:0.0}");
                    rowValues.Add($"{yearwiseobjects.June:0.0}");
                    rowValues.Add($"{yearwiseobjects.July:0.0}");
                    rowValues.Add($"{yearwiseobjects.August:0.0}");
                    rowValues.Add($"{yearwiseobjects.September:0.0}");
                    rowValues.Add($"{yearwiseobjects.October:0.0}");
                    rowValues.Add($"{yearwiseobjects.November:0.0}");
                    rowValues.Add($"{yearwiseobjects.December:0.0}");

                    yearwisereport.AddRow(rowValues.ToArray());

                }
                AnsiConsole.Write(yearwisereport);
            }
        }
        public static void GetYearReport(int year)
        {
            // Modified query to filter for the specific year
            string pivotQuery = @"
    WITH YearTable AS (
        SELECT 
            StackName, 
            YEAR(DateOfSession) AS sessionyear,
            MONTH(DateOfSession) AS sessionmonth, 
            AVG(score) AS averagescore 
        FROM studysessions
        WHERE YEAR(DateOfSession) = @Year  -- Filter for the requested year
        GROUP BY StackName, YEAR(DateOfSession), MONTH(DateOfSession)  -- Proper grouping for monthly averages
    )
    SELECT 
        StackName,
        sessionyear, 
        ISNULL([1], 0) AS January,
        ISNULL([2], 0) AS February,
        ISNULL([3], 0) AS March,
        ISNULL([4], 0) AS April,
        ISNULL([5], 0) AS May,
        ISNULL([6], 0) AS June,
        ISNULL([7], 0) AS July,
        ISNULL([8], 0) AS August,
        ISNULL([9], 0) AS September,
        ISNULL([10], 0) AS October,
        ISNULL([11], 0) AS November,
        ISNULL([12], 0) AS December
    FROM YearTable
    PIVOT (
        MAX(averagescore)
        FOR sessionmonth IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
    ) AS PivotTable
    ORDER BY StackName;";

            List<FinalReport> yearReports;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                yearReports = conn.Query<FinalReport>(pivotQuery, new { Year = year }).ToList();
            }

            if (yearReports.Count == 0)
            {
                AnsiConsole.MarkupLine($"[red]No study sessions available for year {year}[/]");
                return;
            }

            // Create and display the table
            var reportTable = new Table();
            reportTable.Title = new TableTitle($"Consolidated Report for {year}");
            reportTable.Border = TableBorder.Rounded;

            // Define columns in desired order
            var columns = new[]
            {
        "StackName", "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    };

            // Add columns
            reportTable.AddColumn("[bold]Stack[/]");
            foreach (var month in columns.Skip(1))
            {
                reportTable.AddColumn($"[bold]{month}[/]");
            }

            // Add rows
            foreach (var report in yearReports.OrderBy(r => r.StackName))
            {
                var rowValues = new List<string> { report.StackName };

                // Add all month values formatted to 1 decimal place
                rowValues.Add($"{report.January:0.0}");
                rowValues.Add($"{report.February:0.0}");
                rowValues.Add($"{report.March:0.0}");
                rowValues.Add($"{report.April:0.0}");
                rowValues.Add($"{report.May:0.0}");
                rowValues.Add($"{report.June:0.0}");
                rowValues.Add($"{report.July:0.0}");
                rowValues.Add($"{report.August:0.0}");
                rowValues.Add($"{report.September:0.0}");
                rowValues.Add($"{report.October:0.0}");
                rowValues.Add($"{report.November:0.0}");
                rowValues.Add($"{report.December:0.0}");

                reportTable.AddRow(rowValues.ToArray());
            }

            AnsiConsole.Write(reportTable);
        }
    }
}
