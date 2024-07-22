using Dapper;
using System.Data.SqlClient;
using Flashcards.Arashi256.Models;
using Flashcards.Arashi256.Config;

namespace Flashcards.Arashi256.Classes
{
    internal class StacksDatabase
    {
        private Database _database;
        private DatabaseConnection _connection;

        public StacksDatabase()
        { 
            _database = new Database();
            _database.TestConnection();
            _connection = _database.GetConnection();
        }

        public List<Stack> GetStackResults(string query, DynamicParameters? parameters = null)
        {
            List<Stack> results = new List<Stack>();
            using (var connection = new SqlConnection(_connection.DatabaseConnectionString))
            {
                try
                {
                    results = connection.Query<Stack>(query, parameters).AsList();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("SQL ERROR: " + sqlEx.Message);
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    Console.WriteLine("INVALID OPERATION: " + invalidOpEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                }
            }
            return results;
        }

        public bool CheckDuplicateStack(string subject)
        {
            string query = "SELECT Id, Subject FROM dbo.stacks WHERE LOWER(Subject) = LOWER(@Subject)";
            var param = new DynamicParameters();
            param.Add("@Subject", subject);
            List<Stack> stacks = GetStackResults(query, param);
            return stacks.Count > 0 ? false : true;
        }

        public int AddNewStack(Stack s)
        {
            var sql = "INSERT INTO dbo.stacks (Subject) VALUES (@Subject)";
            var parameters = new DynamicParameters();
            parameters.Add("@Subject", s.Subject);
            return _database.ExecuteQuery(sql, parameters);
        }

        public int UpdateExistingStack(Stack s)
        {
            var sql = "UPDATE dbo.stacks SET Subject = @Subject WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Subject", s.Subject);
            parameters.Add("@Id", s.Id);
            return _database.ExecuteQuery(sql, parameters);
        }

        public int DeleteExistingStack(Stack s)
        {
            var sql = "DELETE FROM dbo.stacks WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", s.Id);
            return _database.ExecuteQuery(sql, parameters);
        }
    }
}
