using Dapper;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;

namespace FlashCards


{
    internal class CardStackRepository : IRepository<CardStack>
    {
        public string ConnectionString { get; }
        public string Table { get; } = "Stacks";
        public bool DoesTableExist()
        {
            try
            {
                string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Stacks';";
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
                string sql = "CREATE TABLE Stacks (" +
                "StackID int NOT NULL IDENTITY(1,1) PRIMARY KEY," +
                "StackName varchar(50) NOT NULL," +
                "CONSTRAINT UK_StackName UNIQUE (StackName)" +
                ");";

                ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while creating the table");
                throw;
            }
            

        }
        public void AutoFill(List<CardStack> defaultData)
        {
            try
            { 
                string sql = "INSERT INTO [Stacks] ([StackName]) VALUES (@StackName);";

                using var connection = new SqlConnection(ConnectionString);

                foreach (CardStack stack in defaultData)
                {
                    connection.Execute(sql, stack);
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

        public CardStackRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public bool Insert(CardStack entity)
        {
            try
            {
                string sqlCommand = "INSERT INTO [Stacks] ([StackName]) VALUES (@StackName);";
                var result = ExecuteNonQuery(sqlCommand, entity);

                return result > 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Database error while inserting: {entity.StackName}");
                Console.WriteLine("Please review all rules and try again");
                return false;

            }
        }

        public bool Update(CardStack entity)
        {
            try
            {
                string sqlCommand = "UPDATE [Stacks] SET StackName=@StackName WHERE StackID=@StackID;";
                var result = ExecuteNonQuery(sqlCommand, entity);

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error while inserting: {entity.StackName}");
                Console.WriteLine("Please review all rules and try again");
                return false;
            }
        }

        public bool Delete(CardStack entity)
        {
            try
            {
                string sqlCommand = "DELETE FROM [Stacks]  WHERE StackID=@StackID;";
                var result = ExecuteNonQuery(sqlCommand, entity);

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error ocwhile updating: {entity.StackName}");
                Console.WriteLine("Please review all rules and try again");
                return false;
            }
        }

        public IEnumerable<CardStack> GetAllRecords()
        {
            try
            {
                string sqlCommand = "SELECT * FROM [Stacks];";
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Query<CardStack>(sqlCommand);
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

        private int ExecuteNonQuery(string sqlCommand, CardStack entity) 
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Execute(sqlCommand, entity);

                return result;
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
        private int ExecuteNonQuery(string sqlCommand)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Execute(sqlCommand);
                return result;
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

    }
}
