using Dapper;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;

namespace FlashCards


{
    internal class CardStackRepository : IRepository<CardStack>
    {
        public string ConnectionString { get; }

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

    }
}
