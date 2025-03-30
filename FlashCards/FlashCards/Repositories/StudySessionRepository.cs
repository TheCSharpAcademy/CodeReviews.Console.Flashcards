using Dapper;
using Microsoft.Data.SqlClient;


namespace FlashCards
{
    internal class StudySessionRepository
    {

        
        public string ConnectionString { get; }

        public StudySessionRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
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
                Console.WriteLine($"Database error while checking if table exists");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while checking if table exists");
                throw;
            }

        }
        
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
                Console.WriteLine($"Database error while executing the command");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while executing the command");
                throw;
            }
        }
        
        public void AutoFill(List<CardStack> stacks, List<StudySession> sessions)
        {
            try
            {
                string sql = "INSERT INTO [StudySessions] VALUES (@StackID, @SessionDate, @Score);";

                using var connection = new SqlConnection(ConnectionString);

                foreach (var session in sessions)
                {
                    session.StackId = stacks.FirstOrDefault(x => x.StackName == session.StackName).StackID;
                    connection.Execute(sql, session);
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while autofiling the database");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while autofiling the database");
                throw;
            }

        }
        
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
                Console.WriteLine($"Database error while executing the command");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while executing the command");
                throw;
            }

        }
        //END
        public IEnumerable<StudySession> GetAllRecords()
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
                Console.WriteLine($"Database error while executing the command");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while executing the command");
            }
            return new List<StudySession>();
        }
        
        public string GetReportSqlCommand()
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
        public string GetReportPivotFunction(PivotFunction pivotFunction)
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
               };
        }
        public ReportObject? GetDataPerMonthInYear(CardStack stack, int year, PivotFunction pivotFunction)
        {
            string sql = GetReportSqlCommand() + GetReportPivotFunction(pivotFunction);

            try
            {
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.QueryFirstOrDefault<ReportObject>(sql, new {Year = year, StackID = stack.StackID});
                return result;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database error while executing the command");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while executing the command");
            }
            return null;
        }
    }
}
