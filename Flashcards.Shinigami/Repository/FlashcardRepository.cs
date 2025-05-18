using Dapper;
using Flashcards.Models;
using Flashcards.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Repository
{
    internal class FlashcardRepository : IFlashcardRepository
    {
        private readonly string? _connectionString;
        public FlashcardRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("defaultConnection");
        }
        public void AddFlashcard(int stackId, string question, string answer)
        {
            string sqlQuery = 
                $@"
                INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId);
                ";
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            dbConnection.Execute(sqlQuery, new {Question = question, Answer = answer, StackId = stackId});
        }

        public void DeleteFlashcard(int flashcardId)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Execute("DELETE FROM Flashcards WHERE Id = @Id", new {Id = flashcardId});
        }

        public List<FlashcardDTO> GetFlashcardsByStack(int stackId)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            return conn.Query<FlashcardDTO>("SELECT Id, Question, Answer FROM Flashcards Where StackId = @StackId", new { StackId = stackId }).ToList();
        }
    }
}
