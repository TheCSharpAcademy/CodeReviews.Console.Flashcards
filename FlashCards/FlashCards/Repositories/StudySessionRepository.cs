using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace FlashCards
{
    /// <inheritdoc/>
    internal class StudySessionRepository : IStudySessionRepository
    {
        /// <inheritdoc/>
        public string ConnectionString { get; }

        /// <summary>
        /// Initializes new instance of StudySessionRepository class
        /// </summary>
        /// <param name="connectionString">A string representing connection string to the database</param>
        public StudySessionRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        /// <inheritdoc/>
        public bool DoesTableExist()
        {
            try
            {
                string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessions';";
                using var connection = new SqlConnection(ConnectionString);
                string? result = connection.Query<string>(sql).FirstOrDefault();

                return result != null;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while checking if table exists\n");
                Console.WriteLine(sqlEx.Message + "\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while checking if table existsn\n");
                Console.WriteLine(ex.Message + "\n");
                throw;
            }

        }
        /// <inheritdoc/>
        public void CreateTable()
        {
            try
            {
                string sql = "CREATE TABLE StudySessions(" +
                "StackID int NOT NULL FOREIGN KEY (StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE," +
                "SessionDate DATE NOT NULL," +
                "Score int NOT NULL" +
                ");";
                using var connection = new SqlConnection(ConnectionString);
                connection.Execute(sql);

            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while creating the table\n");
                Console.WriteLine(sqlEx.Message + "\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while creating the table\n");
                Console.WriteLine(ex.Message + "\n");
                throw;
            }
        }
        /// <inheritdoc/>
        public void AutoFill(List<CardStack> stacks, List<StudySession> sessions)
        {
            try
            {
                string sql = "INSERT INTO [StudySessions] VALUES (@StackID, @SessionDate, @Score);";

                using var connection = new SqlConnection(ConnectionString);

                Dictionary<string, int> stackIdToNameMap = stacks.ToDictionary(x => x.StackName, x => x.StackID);

                foreach (var session in sessions)
                {
                    session.StackId = stackIdToNameMap[session.StackName];
                    connection.Execute(sql, session);
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while auto filling the table\n");
                Console.WriteLine(sqlEx.Message + "\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while auto filling the table\n");
                Console.WriteLine(ex.Message + "\n");
                throw;
            }

        }
        /// <inheritdoc/>
        public bool Insert(StudySession entity)
        {

            try
            {
                string sql = "INSERT INTO [StudySessions] VALUES (@StackID, @SessionDate, @Score);";
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Execute(sql, entity);

                return result > 0;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while inserting {entity.SessionDate}|{entity.StackName}\n");
                Console.WriteLine(sqlEx.Message + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while inserting {entity.SessionDate}|{entity.StackName}\n");
                Console.WriteLine(ex.Message + "\n");
                return false;
            }

        }
        /// <inheritdoc/>
        public IEnumerable<StudySession>? GetAllRecords()
        {
            try
            {
                string sqlCommand = "SELECT * FROM [StudySessions] ORDER BY [SessionDate];";
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Query<StudySession>(sqlCommand);
                return result;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while getting all records\n");
                Console.WriteLine(sqlEx.Message + "\n");

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while getting all records\n");
                Console.WriteLine(ex.Message + "\n");

                return null;
            }
        }
        /// <summary>
        /// Retrieves SQL command for the report
        /// </summary>
        /// <returns>A string representing SQL command for the report</returns>
        private string GetReportSqlCommand()
        {
            return "SELECT [1] AS January," +
                       "[2] AS February," +
                       "[3] AS March," +
                       "[4] AS April," +
                       "[5] AS May," +
                       "[6] AS June," +
                       "[7] AS July, " +
                       "[8] AS August," +
                       "[9] AS September, " +
                       "[10] AS October," +
                       "[11] AS November," +
                       "[12] AS December " +
                "FROM (" +
                    "SELECT MONTH(SessionDate) as SessionMonth, Score " +
                    "FROM(" +
                        "SELECT * FROM [StudySessions] WHERE YEAR(SessionDate) = @Year AND StackID=@StackID" +
                        ")[StudySessions]" +
                        ")[StudySessions]";
        }
        /// <summary>
        /// Retrieves Pivot function SQL command based on passed parameter
        /// </summary>
        /// <param name="pivotFunction">A Enumerable PivotFunction representing the command</param>
        /// <returns>A string representing SQL command for the pivot function</returns>
        private string GetReportPivotFunction(PivotFunction pivotFunction)
        {
            return pivotFunction switch
            {
                PivotFunction.Count => "PIVOT(" +
                    "COUNT(Score)" +
                    "FOR SessionMonth IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])" +
                ") AS pivot_table;",
                PivotFunction.Average => "PIVOT(" +
                    "AVG(Score)" +
                    "FOR SessionMonth IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])" +
                ") AS pivot_table;",
                PivotFunction.Sum => "PIVOT(" +
                    "SUM(Score)" +
                    "FOR SessionMonth IN ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12])" +
                ") AS pivot_table;",
                _ => throw new InvalidEnumArgumentException("Invalid PivotFunction enum value passed to GetReportPivotFunction()")
            };
        }
        /// <inheritdoc/>
        public ReportObject? GetDataPerMonthInYear(CardStack stack, int year, PivotFunction pivotFunction)
        {
            try
            {
                string sql = GetReportSqlCommand() + GetReportPivotFunction(pivotFunction);
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.QueryFirstOrDefault<ReportObject>(sql, new { Year = year, StackID = stack.StackID });
                return result;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while getting report object for {stack.StackName}|{year}|{pivotFunction}\n");
                Console.WriteLine(sqlEx.Message + "\n");

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while getting report object for {stack.StackName}|{year}|{pivotFunction}\n");
                Console.WriteLine(ex.Message + "\n");

                return null;
            }
        }
    }
}
