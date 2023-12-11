using System.Data;
using System.Data.SqlClient;

namespace Flashcards.UgniusFalze.Models
{
    public class Flashcards
    {
        public int StackId { get; set; }
        public int FlashcardId {  get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public Flashcards(int stackId, int flashCardId, string front, string back) {
            StackId = stackId;
            FlashcardId = flashCardId;
            Front = front;
            Back = back;
        }

        public FlashcardDTO ConvertToDto(int order)
        {
            return new FlashcardDTO(FlashcardId, Front, Back, order);
        }
        
        public static bool InsertFlashcard(SqlConnection sqlConn, string front, string back, int stackId)
        {
            SqlCommand sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandText = "INSERT INTO dbo.Flashcards (Front, Back, StackId) VALUES (@front, @back, @stackId)";
            sqlCommand.Parameters.Add("@front", SqlDbType.NVarChar, 255).Value = front;
            sqlCommand.Parameters.Add("@back", SqlDbType.NVarChar, 255).Value = back;
            sqlCommand.Parameters.Add("@stackId", SqlDbType.Int).Value = stackId;
            sqlConn.Open();
            sqlCommand.Prepare();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
                {
                    sqlConn.Close();
                    return false;
                }
                else
                {
                    throw e;
                }
            }
            sqlConn.Close();
            return true;
        }
        public void DeleteFlashcard(SqlConnection sqlConn)
        {
            SqlCommand sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandText = "DELETE FROM dbo.Flashcards WHERE FlashcardId = @fid";
            sqlCommand.Parameters.Add("@fid", SqlDbType.Int).Value = FlashcardId;
            sqlConn.Open();
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            sqlConn.Close();
        }
        
        public void UpdateFlashcard(SqlConnection sqlConn)
        {
            SqlCommand sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandText = "UPDATE dbo.Flashcards SET Front = @front, Back = @back WHERE FlashcardId = @flashcardId";
            sqlCommand.Parameters.Add("@front", SqlDbType.Int).Value = Front;
            sqlCommand.Parameters.Add("@back", SqlDbType.Int).Value = Back;
            sqlCommand.Parameters.Add("@flashcardId", SqlDbType.Int).Value = FlashcardId;
            sqlConn.Open();
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            sqlConn.Close();
        }
    }
}
