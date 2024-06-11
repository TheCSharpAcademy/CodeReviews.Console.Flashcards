using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Flashcards.UndercoverDev.Models;

namespace Flashcards.UndercoverDev.Repository
{
    public class StackRepository : IStackRepository
    {
        const string _databaseName = "FlashcardDB";
        private readonly string _connectionStringTemplate = ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
        private readonly string _connectionString;
        public StackRepository()
        {
            _connectionString = string.Format(_connectionStringTemplate, _databaseName);
        }

        public void Post(StackDTO stack)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string createStackQuery = $"INSERT INTO Stack(Name) VALUES('{stack.Name}');";
            connection.Execute(createStackQuery);
        }

        public void Put(string stackName, string newStackName)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string updateStackQuery = $"UPDATE Stack SET Name = '{newStackName}' WHERE Name = '{stackName}';";
            connection.Execute(updateStackQuery);
        }

        public void Delete(Stack stack)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string deleteStackQuery = $"DELETE FROM Stack WHERE Name = @Name;";
            connection.Execute(deleteStackQuery, stack);
        }

        public List<Stack> GetStacks()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string getStacksQuery = $"SELECT * FROM Stack;";

            return connection.Query<Stack>(getStacksQuery).ToList();
        }

        public List<string> GetStackNames()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var stackNames = connection.Query<string>("SELECT Name FROM Stack").ToList();
            stackNames.Insert(0, "Back");
            return stackNames;
        }

        public Stack GetStackByName(string name)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string getStackQuery = $"SELECT * FROM Stack WHERE Name = '{name}';";
            return connection.Query<Stack>(getStackQuery).FirstOrDefault();
        }

        public Stack GetStackById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            string getStackQuery = $"SELECT * FROM Stack WHERE Id = '{id}';";
            return connection.Query<Stack>(getStackQuery).FirstOrDefault();
        }
    }
}