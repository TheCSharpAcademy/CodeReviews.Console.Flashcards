using Dapper;
using Flashcards.DAL.DTO;
using Microsoft.Data.SqlClient;

namespace Flashcards.DAL
{
    public class Repository
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["flashcards"].ConnectionString;

        public bool ExecuteQuery(string sql, object? parameters = null)
        {
            int success;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                success = connection.Execute(sql, parameters);
                connection.Close();
            }

            return success != 0;
        }

        public bool Insert<T>(string tableName, T model)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(p => p.Name != "ID")
                .ToList();

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            string sql = $@"
            INSERT INTO 
                {tableName} ({columnNames})
            VALUES 
                ({paramNames})";

            return ExecuteQuery(sql, model);
        }

        public bool Update<T>(string tableName, T model)
        {
            var properties = typeof(T)
                .GetProperties()
                .ToList();

            var keyProperty = properties.First(p => p.Name.Equals("ID"));

            properties = properties
                .Where(p => p != keyProperty)
                .ToList();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            string sql = $@"
            UPDATE 
                {tableName}
            SET 
                {setClause}
            WHERE
                {keyProperty.Name} = @{keyProperty.Name}";

            return ExecuteQuery(sql, model);
        }

        public bool Delete(string tableName, int id)
        {
            var parameters = new { Id = id };

            string sql = $@"
            DELETE FROM
                {tableName}
            WHERE
                ID = @ID";

            return ExecuteQuery(sql, parameters);
        }

        public T ExecuteQuerySingle<T>(string sql, object parameters)
        {
            T result;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                result = connection.QuerySingle<T>(sql, parameters);
                connection.Close();
            }

            return result;
        }

        public FlashcardStackDTO GetFlashcardByID(int id)
        {
            var parameters = new { Id = id };

            string sql = $@"
            SELECT
                Flashcard.ID,
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID
            WHERE
                Flashcard.ID = @ID";


            return ExecuteQuerySingle<FlashcardStackDTO>(sql, parameters);
        }

        public List<FlashcardStackDTO> GetStackByName(string name)
        {
            List<FlashcardStackDTO> flashCards = new List<FlashcardStackDTO>();
            var parameters = new { Name = name };
            string sql = $@"
            SELECT
                ROW_NUMBER() OVER (ORDER BY Flashcard.Front) AS ID,
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID
            WHERE
                Stack.Name = @Name";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                flashCards = connection.Query<FlashcardStackDTO>(sql, parameters).ToList();
                connection.Close();
            }

            return flashCards;
        }

        public int GetStackIDByName(string name)
        {
            int id;
            var parameters = new { Name = name };
            string sql = $@"
            SELECT
                Stack.ID
            FROM
                Stack
            WHERE
                Stack.Name = @Name";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                id = connection.ExecuteScalar<int>(sql, parameters);
                connection.Close();
            }

            return id;
        }

        public List<FlashcardStackDTO> GetAllFlashcards()
        {
            List<FlashcardStackDTO> flashCards = new List<FlashcardStackDTO>();
            string sql = $@"
            SELECT
                Flashcard.ID,
                Flashcard.Front,
                Flashcard.Back,
                Stack.Name AS StackName
            FROM
                Flashcard INNER JOIN
                Stack ON Stack.ID = Flashcard.StackID";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                flashCards = connection.Query<FlashcardStackDTO>(sql).ToList();
                connection.Close();
            }

            return flashCards;
        }

        public List<StackDTO> GetAllStacks()
        {
            List<StackDTO> stacks = new List<StackDTO>();
            string sql = "SELECT Stack.Name FROM Stack";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                stacks = connection.Query<StackDTO>(sql).ToList();
                connection.Close();
            }

            List<FlashcardStackDTO> flashcards = GetAllFlashcards();
            foreach (FlashcardStackDTO flashcard in flashcards)
            {
                stacks.Where(s => s.Name == flashcard.StackName).ToList().ForEach(s => s.Flashcards.Add(flashcard));
            }

            return stacks;
        }

        public List<StudySessionDTO> GetAllStudySessions()
        {
            List<StudySessionDTO> studySessions = new List<StudySessionDTO>();
            string sql = @$"SELECT
                StudySession.ID,
	            StudySession.Date,
	            StudySession.Score,
	            Stack.Name AS StackName
            FROM
	            StudySession INNER JOIN
	            Stack ON Stack.ID = StudySession.StackID";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                studySessions = connection.Query<StudySessionDTO>(sql).ToList();
                connection.Close();
            }

            return studySessions;
        }

        public List<StudySessionPerMonthDTO> GetStudySessionsPerMonthPerStack(int year)
        {
            List<StudySessionPerMonthDTO> studySessions = new List<StudySessionPerMonthDTO>();
            var parameters = new { Year = "2025" };
            string sql = @"SELECT *
                FROM (
                    SELECT 
                        DATENAME(MONTH, [Date]) AS Months,
                        Score,
                        Stack.Name AS StackName
                    FROM StudySession 
                    INNER JOIN Stack ON Stack.ID = StudySession.StackID
	                WHERE YEAR([Date]) = @Year
                ) AS SourceTable
                PIVOT (
                    COUNT(Score)
                    FOR Months IN ([January], [February], [March], [April], [May], [June],
                                   [July], [August], [September], [October], [November], [December])
                ) AS PivotTable;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                studySessions = connection.Query<StudySessionPerMonthDTO>(sql, parameters).ToList();
                connection.Close();
            }

            return studySessions;
        }

        public bool StackNameExists(string name)
        {
            int count;
            string sql = "SELECT COUNT(1) FROM Stack WHERE Name = @Name";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                count = connection.ExecuteScalar<int>(sql, new { Name = name });
                connection.Close();
            }

            return count > 0;
        }

        public bool FlashcardIDExists(int id)
        {
            int count;
            string sql = "SELECT COUNT(1) FROM Flashcard WHERE ID = @ID";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                count = connection.ExecuteScalar<int>(sql, new { ID = id });
                connection.Close();
            }

            return count > 0;
        }
    }
}
