using System.Data;
using System.Data.SqlClient;

namespace Flashcards.UgniusFalze.Models
{
    public class Stacks
    {
        public int StackId { get; set; }
        public string StackName { get; set; }
        public Stacks(int stackId, string stackName) { 
            StackId = stackId;
            StackName = stackName;
        }
        
        public static bool InsertStack(SqlConnection sqlConn, string stackName)
        {
            SqlCommand sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandText = "INSERT INTO dbo.Stack (StackName) VALUES (@stackName)";
            sqlCommand.Parameters.Add("@stackName", SqlDbType.NVarChar, 255).Value = stackName;
            sqlConn.Open();
            sqlCommand.Prepare();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    sqlConn.Close();
                    return false;
                } 
                throw e;
            }
            sqlConn.Close();
            return true;
        }
        
        public void DeleteStack(SqlConnection sqlConn)
        {
            SqlCommand sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandText = "DELETE FROM dbo.Stack WHERE StackId = @sid";
            sqlCommand.Parameters.Add("@sid", SqlDbType.Int).Value = StackId;
            sqlConn.Open();
            sqlCommand.Prepare();
            sqlCommand.ExecuteNonQuery();
            sqlConn.Close();
        }

        public bool UpdateStackName(string stackName, SqlConnection sqlConn)
        {
            SqlCommand sqlCommand = sqlConn.CreateCommand();
            sqlCommand.CommandText = "UPDATE dbo.Stack SET StackName = @stackName WHERE StackId = @stackId";
            sqlCommand.Parameters.Add("@stackName", SqlDbType.NVarChar, 255).Value = stackName;
            sqlCommand.Parameters.Add("@stackId", SqlDbType.NVarChar, 255).Value = StackId;
            sqlConn.Open();
            sqlCommand.Prepare();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (e.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    sqlConn.Close();
                    return false;
                } 
                throw e;
            }
            sqlConn.Close();
            return true;
        }
    }
}
