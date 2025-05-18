using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Repository
{
    public class StackRepository : IStackRepository
    {
        public readonly string? _connectionstring;

        public StackRepository(IConfiguration config)
        {
            _connectionstring = config.GetConnectionString("defaultConnection");
        }

        public void AddStack(string stackName)
        {
            using IDbConnection conn = new SqlConnection(_connectionstring);
            conn.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", new {Name = stackName});
        }

        public void DeleteStack(int stackId)
        {
            using IDbConnection conn = new SqlConnection(_connectionstring);
            conn.Execute("DELETE FROM Stacks WHERE Id = @Id", new { Id = stackId });
        }

        public List<Models.Stack> GetAllStacks()
        {
            using IDbConnection conn = new SqlConnection(_connectionstring);
            return conn.Query<Models.Stack>("SELECT * FROM Stacks ORDER BY Name").ToList();
        }

        public Models.Stack? GetStackById(int stackId)
        {
            using IDbConnection conn = new SqlConnection(_connectionstring);
            return conn.QuerySingleOrDefault<Models.Stack>("SELECT * FROM Stacks WHERE Id = @Id", new {Id = stackId});
        }
    }
}
