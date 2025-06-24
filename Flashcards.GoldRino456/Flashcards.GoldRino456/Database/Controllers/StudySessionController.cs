using Dapper;
using Utilities.GoldRino456;
using Flashcards.GoldRino456.Database.Models;

namespace Flashcards.GoldRino456.Database.Controllers
{
    internal class StudySessionController: IDatabaseTable<StudySession>
    {
        private readonly string _connectionString;

        public StudySessionController(string connectionString)
        {
            _connectionString = connectionString;
            CreateTable();
        }

        private void CreateTable()
        {
            string createQuery = "IF OBJECT_ID(N'Sessions', N'U') IS NULL CREATE TABLE Sessions (sessionId INT IDENTITY(1,1) PRIMARY KEY, stackId INT FOREIGN KEY REFERENCES Stacks(stackID), sessionDate DATE, score INT);";

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, createQuery);
        }

        public void CreateEntry(StudySession newEntry)
        {
            string createQuery = "INSERT INTO Sessions (stackId, sessionDate, score) VALUES (@StackId, @SessionDate, @Score);";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StackId", newEntry.StackId);
            parameters.Add("@SessionDate", newEntry.SessionDate);
            parameters.Add("@Score", newEntry.Score);

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, createQuery, parameters);
        }

        public void DeleteEntry(int id)
        {
            string deleteQuery = "DELETE FROM Sessions WHERE sessionId = @Id;";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, deleteQuery, parameters);
        }

        public List<StudySession> ReadAllEntries()
        {
            string readQuery = "SELECT * FROM Sessions";

            return DatabaseUtils.ExecuteQueryCommand<StudySession>(_connectionString, readQuery);
        }

        public List<StudySession> ReadAllEntriesFromStack(int stackId)
        {
            string readQuery = "SELECT * FROM Sessions WHERE stackId = @StackId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StackId", stackId);

            return DatabaseUtils.ExecuteQueryCommand<StudySession>(_connectionString, readQuery, parameters);
        }

        public StudySession ReadEntry(int id)
        {
            string readQuery = "SELECT * FROM Sessions WHERE sessionId = @Id";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return DatabaseUtils.ExecuteQueryCommand<StudySession>(_connectionString, readQuery, parameters).First();
        }

        public void UpdateEntry(int id, StudySession updatedEntry)
        {
            string updateQuery = "UPDATE Sessions SET stackId = @StackId, sessionDate = @SessionDate, score = @Score WHERE sessionId = @Id";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@StackId", updatedEntry.StackId);
            parameters.Add("@SessionDate", updatedEntry.SessionDate);
            parameters.Add("@Score", updatedEntry.Score);

            DatabaseUtils.ExecuteNonQueryCommand(_connectionString, updateQuery, parameters);
        }
    }
}
