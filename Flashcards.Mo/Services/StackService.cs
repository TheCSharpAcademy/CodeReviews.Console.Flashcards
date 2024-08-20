using Flashcards.Domain.Entities;
using Flashcards.Domain.Interfaces;
using ConsoleTables;
using Npgsql;


namespace Flashcards.Services
{
    public class StackService
    {
        private readonly IStackRepository _stackRepository;
        private readonly IFlashcardRepository _flashcardRepository;
        private readonly IStudySessionRepository _studySessionRepository;
        private readonly string _connectionString;

         public StackService(IStackRepository stackRepository, IFlashcardRepository flashcardRepository, IStudySessionRepository studySessionRepository, string connectionString)
    {
        _stackRepository = stackRepository;
        _flashcardRepository = flashcardRepository;
        _studySessionRepository = studySessionRepository;
        _connectionString = connectionString;  
    }

        public Stack? GetStackByName(string name)
        {
            return _stackRepository.GetByName(name);
        }

        public IEnumerable<Stack> GetAllStacks()
        {
            return _stackRepository.GetAll();
        }

        public void CreateStack(string name)
        {
            var existingStack = _stackRepository.GetByName(name);
            if (existingStack != null)
            {
                throw new InvalidOperationException("Stack name must be unique.");
            }

            var stack = new Stack { Name = name };
            _stackRepository.Add(stack);
        }

        public void DeleteStackByName(string name)
        {
            var stack = _stackRepository.GetByName(name);
            if (stack != null)
            {
                _flashcardRepository.DeleteByStackId(stack.Id);
                _studySessionRepository.DeleteByStackId(stack.Id);
                _stackRepository.Delete(stack.Id);
            }
        }

        public IEnumerable<Flashcard> GetFlashcardsForStack(int stackId)
        {
            return _stackRepository.GetById(stackId)?.Flashcards ?? new List<Flashcard>();
        }

        public void ShowSessionsPerMonth(int year)
        {
            var query = @"
        SELECT
            s.""Name"" AS StackName,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 1 THEN 1 ELSE 0 END) AS January,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 2 THEN 1 ELSE 0 END) AS February,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 3 THEN 1 ELSE 0 END) AS March,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 4 THEN 1 ELSE 0 END) AS April,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 5 THEN 1 ELSE 0 END) AS May,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 6 THEN 1 ELSE 0 END) AS June,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 7 THEN 1 ELSE 0 END) AS July,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 8 THEN 1 ELSE 0 END) AS August,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 9 THEN 1 ELSE 0 END) AS September,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 10 THEN 1 ELSE 0 END) AS October,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 11 THEN 1 ELSE 0 END) AS November,
            SUM(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 12 THEN 1 ELSE 0 END) AS December
        FROM
            ""StudySessions"" ss
        JOIN
            ""Stacks"" s ON s.""Id"" = ss.""StackId""
        WHERE
            EXTRACT(YEAR FROM ss.""Date"") = @Year
        GROUP BY
            s.""Name""
        ORDER BY
            s.""Name"";
    ";

            using (var connection = new NpgsqlConnection(_connectionString))  
            {
                connection.Open();  

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", year);

                    using (var reader = command.ExecuteReader())
                    {
                        var table = new ConsoleTable("StackName", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");

                        while (reader.Read())
                        {
                            table.AddRow(
                                reader["StackName"],
                                reader["January"],
                                reader["February"],
                                reader["March"],
                                reader["April"],
                                reader["May"],
                                reader["June"],
                                reader["July"],
                                reader["August"],
                                reader["September"],
                                reader["October"],
                                reader["November"],
                                reader["December"]
                            );
                        }

                        table.Write(Format.Alternative);
                        Console.WriteLine();
                    }
                }

                connection.Close();  
            }
        }

        public void ShowAverageScorePerMonth(int year)
        {
            var query = @"
        SELECT
            s.""Name"" AS StackName,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 1 THEN ss.""Score"" ELSE NULL END) AS January,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 2 THEN ss.""Score"" ELSE NULL END) AS February,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 3 THEN ss.""Score"" ELSE NULL END) AS March,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 4 THEN ss.""Score"" ELSE NULL END) AS April,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 5 THEN ss.""Score"" ELSE NULL END) AS May,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 6 THEN ss.""Score"" ELSE NULL END) AS June,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 7 THEN ss.""Score"" ELSE NULL END) AS July,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 8 THEN ss.""Score"" ELSE NULL END) AS August,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 9 THEN ss.""Score"" ELSE NULL END) AS September,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 10 THEN ss.""Score"" ELSE NULL END) AS October,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 11 THEN ss.""Score"" ELSE NULL END) AS November,
            AVG(CASE WHEN EXTRACT(MONTH FROM ss.""Date"") = 12 THEN ss.""Score"" ELSE NULL END) AS December
        FROM
            ""StudySessions"" ss
        JOIN
            ""Stacks"" s ON s.""Id"" = ss.""StackId""
        WHERE
            EXTRACT(YEAR FROM ss.""Date"") = @Year
        GROUP BY
            s.""Name""
        ORDER BY
            s.""Name"";
    ";

            using (var connection = new NpgsqlConnection(_connectionString))  
            {
                connection.Open();  

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", year);

                    using (var reader = command.ExecuteReader())
                    {
                        var table = new ConsoleTable("StackName", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");

                        while (reader.Read())
                        {
                            table.AddRow(
                                reader["StackName"],
                                reader["January"] != DBNull.Value ? reader["January"] : "N/A",
                                reader["February"] != DBNull.Value ? reader["February"] : "N/A",
                                reader["March"] != DBNull.Value ? reader["March"] : "N/A",
                                reader["April"] != DBNull.Value ? reader["April"] : "N/A",
                                reader["May"] != DBNull.Value ? reader["May"] : "N/A",
                                reader["June"] != DBNull.Value ? reader["June"] : "N/A",
                                reader["July"] != DBNull.Value ? reader["July"] : "N/A",
                                reader["August"] != DBNull.Value ? reader["August"] : "N/A",
                                reader["September"] != DBNull.Value ? reader["September"] : "N/A",
                                reader["October"] != DBNull.Value ? reader["October"] : "N/A",
                                reader["November"] != DBNull.Value ? reader["November"] : "N/A",
                                reader["December"] != DBNull.Value ? reader["December"] : "N/A"
                            );
                        }

                        table.Write(Format.Alternative);
                        Console.WriteLine();
                    }
                }

                connection.Close();  
            }
        }
    }
}
