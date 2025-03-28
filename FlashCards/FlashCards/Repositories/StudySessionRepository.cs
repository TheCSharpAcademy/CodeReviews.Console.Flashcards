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
                string sqlCommand = "SELECT * FROM [StudySessions];";
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

            return null;
        }
    }
}
