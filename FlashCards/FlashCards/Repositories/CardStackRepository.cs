using Dapper;
using Microsoft.Data.SqlClient;

namespace FlashCards
{
    /// <inheritdoc/>
    internal class CardStackRepository : ICardStackRepository
    {
        /// <inheritdoc/>
        public string ConnectionString { get; }

        /// <summary>
        /// Initializes new instance of CardStackRepository class
        /// </summary>
        /// <param name="connectionString">A string representing connection string to the database</param>
        public CardStackRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <inheritdoc/>
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
        public bool CreateTable()
        {
            try
            {
                string sql = "CREATE TABLE Stacks (" +
                "StackID int NOT NULL IDENTITY(1,1) PRIMARY KEY," +
                "StackName varchar(50) NOT NULL," +
                "CONSTRAINT UK_StackName UNIQUE (StackName)" +
                ");";

                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Execute(sql);
                return result > 0;
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
                Console.WriteLine($"Database error while inserting {entity.StackName}\n");
                Console.WriteLine(ex.Message + "\n");

                return false;
            }
        }

        /// <inheritdoc/>
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
                Console.WriteLine($"Database error while updating {entity.StackName}\n");
                Console.WriteLine(ex.Message + "\n");

                return false;
            }
        }

        /// <inheritdoc/>
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
                Console.WriteLine($"Database error while deleting {entity.StackName}\n");
                Console.WriteLine(ex.Message + "\n");

                return false;
            }
        }

        /// <inheritdoc/>
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
                Console.WriteLine($"Database error while getting all records\n");
                Console.WriteLine(sqlEx.Message + "\n");

                return Enumerable.Empty<CardStack>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while getting all records\n");
                Console.WriteLine(ex.Message + "\n");

                return Enumerable.Empty<CardStack>();
            }
        }

        /// <summary>
        /// Executes non-query command against the database
        /// </summary>
        /// <param name="sqlCommand">A string representing SQL command</param>
        /// <param name="entity">A CardStack entity for which command will be ran</param>
        /// <returns>A number of rows affected</returns>
        private int ExecuteNonQuery(string sqlCommand, CardStack entity)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Execute(sqlCommand, entity);

                return result;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}