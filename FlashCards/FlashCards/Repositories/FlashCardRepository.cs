using Dapper;
using Microsoft.Data.SqlClient;

namespace FlashCards
{
    internal class FlashCardRepository: IFlashCardRepository
    {
      
        public string ConnectionString { get; }

        public FlashCardRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public bool DoesTableExist()
        {
            try
            {
                string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FlashCards';";
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
                string sql = "CREATE TABLE FlashCards(" +
                "CardID int NOT NULL IDENTITY(1, 1) PRIMARY KEY," +
                "StackID int NOT NULL," +
                "FrontText varchar(50)," +
                "BackText varchar(50)," +
                "CONSTRAINT FK_StackID FOREIGN KEY(StackID) REFERENCES Stacks(StackID) ON DELETE CASCADE" +
                ");";

                ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while creating the table");
                throw;
            }


        }
        public void AutoFill(List<CardStack> stacks, List<FlashCard> flashCards)
        {
            try
            {
                string sql= "INSERT INTO [FlashCards] VALUES (@StackID, @FrontText, @BackText);";

                using var connection = new SqlConnection(ConnectionString);

                foreach (var card in flashCards)
                {
                    card.StackID = stacks.FirstOrDefault(x => x.StackName == card.StackName)!.StackID;
                    connection.Execute(sql, card);
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
        public bool Insert(FlashCard entity)
        {
            try
            {
                string sqlCommand = "INSERT INTO [FlashCards] VALUES (@StackID, @FrontText, @BackText);";
                var result = ExecuteNonQuery(sqlCommand, entity);

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error while inserting flash card");
                return false;

            }
        }
        public bool Update(FlashCard entity)
        {
            try
            {
                string sqlCommand = "UPDATE [FlashCards] SET StackID=@StackID, FrontText=@FrontText, BackText=@BackText WHERE CardID=@CardID;";
                var result = ExecuteNonQuery(sqlCommand, entity);

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error while updating flash card");
                return false;
            }
        }
        public bool Delete(FlashCard entity)
        {
            try
            {
                string sqlCommand = "DELETE FROM [FlashCards] WHERE CardID=@CardID;";
                var result = ExecuteNonQuery(sqlCommand, entity);

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error ocwhile deleting flash card");
                return false;
            }
        }

        public IEnumerable<FlashCard> GetAllRecords()
        {
            try
            {
                string sqlCommand = "SELECT * FROM [FlashCards];";
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Query<FlashCard>(sqlCommand);
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
        public IEnumerable<FlashCardDto> GetAllCardsInStack(CardStack stack)
        {
            try
            {
                string sqlCommand = "SELECT * FROM [FlashCards] WHERE StackID=@stackID;";
                using var connection = new SqlConnection(ConnectionString);
                var result = connection.Query<FlashCardDto>(sqlCommand, stack);
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
        public IEnumerable<FlashCardDto> GetXCardsInStack(CardStack stack, int count)
        {
            string sqlCommand = "SELECT TOP (@Count) [CardID],[FrontText],[BackText] FROM [FlashCards] WHERE StackID=@StackID;";

            using var connection = new SqlConnection(ConnectionString);
            
            var result = connection.Query<FlashCardDto>(sqlCommand, new { Count = count, stackID = stack.StackID });

            return result;
        }


        private int ExecuteNonQuery(string sqlCommand, FlashCard entity)
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
