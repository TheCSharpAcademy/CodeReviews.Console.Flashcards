using Dapper;
using Flashcards.Study.Models;
using Flashcards.Study.Models.Domain;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace Flashcards.Study
{
    public class DatabaseAccess
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["defaultconnection"].ConnectionString;
        public static string connectionString { get; set; }

        public DatabaseAccess()
        {
            connectionString = _connectionstring;
        }

        public static void createDBIfNotExists()
        {
            try
            {
                string query = @"If DB_ID('stacksNflashcards') is null
                              Begin
                               create DataBase stacksNflashcards
                              END;";
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                int row = conn.Execute(query);
                if (row > 0)
                {
                    Console.WriteLine("The dataBase stacksNflashcards is created for you");
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
            connectionString = connectionString + "; Initial Catalog= stacksNflashcards;";
            //Console.ReadLine();
        }

        public static async Task createTables()
        {
            string DBconnectionString = connectionString;
            string stacksquery = @"if object_id('stacks') is null
                                 BEGIN
                                    Create Table stacks(
                                      stackname varchar(20) primary key
                                    );                                    
                                  END";
            string flashcardsquery = @"if object_id('flashcards') is null
                                    BEGIN 
                                        Create Table flashcards(
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
                SqlConnection conn = new SqlConnection(DBconnectionString);
                conn.Open();
                conn.Execute(stacksquery);
                conn.Execute(flashcardsquery);
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
                SqlConnection conn = new SqlConnection(connectionString);
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
                SqlConnection conn = new SqlConnection(connectionString);
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
                SqlConnection conn = new SqlConnection(connectionString);
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
        public static List<FlashcardDTO> GetAllFlashcards()
        {
            List<FlashcardDTO> flashcardDTOs = new List<FlashcardDTO>();
            try
            {
                string query = @"select * from flashcards";
                SqlConnection conn = new SqlConnection(connectionString);
                using (conn)
                {
                    conn.Open();
                    List<flashcard> flashcards = conn.Query<flashcard>(query).ToList();
                    conn.Close();
                    int count = 1;
                    foreach (flashcard fl in flashcards)
                    {
                        flashcardDTOs.Add(new FlashcardDTO
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
            return flashcardDTOs;
        }
        public static void DeleteFlashcard(int id)
        {
            List<flashcard> flashcards;
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                using (conn)
                {
                    conn.Open();
                    flashcards = conn.Query<flashcard>("select * from flashcards").ToList();
                    conn.Execute("delete from flashcards where ID=@id", new { id = flashcards[id - 1].ID });
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static List<FlashcardDTO> GetAllFlashcardsofStack(string stackpicked)
        {
            List<FlashcardDTO> flashcards = null;
            string query = "select * from flashcards where stack_name=@stackname";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    int count = 1;
                    flashcards = conn.Query<flashcard>(query, new { stackname = stackpicked }).Select(x => new FlashcardDTO
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
            return flashcards;
        }
        public static void CreateNewFlashcardforstack(string stackpicked)
        {
            FlashcardDTO newflashcard = UserInputs.CreateNewFlashcard();
            string query = "insert into flashcards([Front],[Back],[stack_name]) values (@front,@back,@stackname)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    conn.Execute(query, new { front = newflashcard.Front, back = newflashcard.Back, stackname = stackpicked });
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void DeleteFlashcardforstack(string stackpicked, List<FlashcardDTO> flashcardsDTO)
        {
            int userChoice = UserInputs.DeleteFlashcardforStack(flashcardsDTO);
            string query = "select * from flashcards where stack_name=@stackname";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                List<flashcard> flashcards = conn.Query<flashcard>(query, new { stackname = stackpicked }).ToList();
                conn.Execute("delete from flashcards where ID=@id", new { id = flashcards[userChoice - 1].ID });
                conn.Close();
            }
        }
        public static void EditFlashcardforstack(string stackpicked, List<FlashcardDTO> flashcardsDTO)
        {
            int userChoice = UserInputs.EditFlashcardforStack(flashcardsDTO);
            string query = "select * from flashcards where stack_name=@stackname";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                List<flashcard> flashcards = conn.Query<flashcard>(query, new { stackname = stackpicked }).ToList();
                var editedFlashCard = UserInputs.GeteditedFlashcardDTO();
                string editQuery = "update flashcards set Front=@front, Back=@back where ID=@id";
                conn.Execute(editQuery, new { front = editedFlashCard.Front, back = editedFlashCard.Back, id = flashcards[userChoice - 1].ID });
            }
        }
        public static Dictionary<string, string> QAOfStacks(string stackpicked)
        {
            Dictionary<string, string> QnA = new Dictionary<string, string>();
            List<FlashcardDTO> cards = GetAllFlashcardsofStack(stackpicked);
            cards.ForEach(x => QnA.Add(x.Front, x.Back));
            return QnA;
        }
        public static void LogStudySession(double userscore, string stackpicked)
        {
            string insertQuery = @"INSERT INTO studysessions([StackName],[score]) Values 
                                  (@stackname,@score)";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                finalreportrows = conn.Query<FinalReport>(pivotQuery).ToList();
            }
            foreach (var yearrow in finalreportrows)
            {
                if (reportdictionary.ContainsKey(yearrow.sessionyear))
                {
                    reportdictionary[yearrow.sessionyear].Add(yearrow);
                }
                else
                {
                    reportdictionary[yearrow.sessionyear] = new List<FinalReport>();
                    reportdictionary[yearrow.sessionyear].Add(yearrow);
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
                    var rowValues = new List<string> { yearwiseobjects.stackname };

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

            using (SqlConnection conn = new SqlConnection(connectionString))
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
            foreach (var report in yearReports.OrderBy(r => r.stackname))
            {
                var rowValues = new List<string> { report.stackname };

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
