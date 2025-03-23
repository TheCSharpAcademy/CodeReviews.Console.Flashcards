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

        
    }
}
