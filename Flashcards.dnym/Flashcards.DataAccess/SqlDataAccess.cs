using Flashcards.DataAccess.DTOs;
using Microsoft.Data.SqlClient;

namespace Flashcards.DataAccess;

public class SqlDataAccess : IDataAccess
{
    private readonly string _connectionString;

    public SqlDataAccess(string connectionString)
    {
        _connectionString = connectionString;

        Task.Run(() =>
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(_connectionString)
                {
                    ConnectTimeout = 5
                };
                using var connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.Clear();
                Console.WriteLine($"Database connection error: {ex.Message}\n\nSuggestion: verify your connection string.\n\nAborting!");
                Environment.Exit(1);
            }
        });
    }

    public async Task<int> CountStacksAsync(int? take = null, int skip = 0)
    {
        var output = 0;

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "count stacks");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_Count_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = reader.GetInt32(0);
            }
        }, "count stacks");

        await connection.CloseAsync();

        return output;
    }

    public async Task<List<StackListItem>> GetStackListAsync(int? take = null, int skip = 0)
    {
        var output = new List<StackListItem>();

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetMultiple_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var stack = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    stack.LastStudied = reader.GetDateTime(3);
                }
                output.Add(stack);
            }
        }, "get stack list");

        await connection.CloseAsync();

        return output;
    }

    public async Task<StackListItem> GetStackListItemByIdAsync(int id)
    {
        StackListItem? output = null;

        var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list item by id");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetById_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", id);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    output.LastStudied = reader.GetDateTime(3);
                }
            }
        }, "get stack list item by id");

        await connection.CloseAsync();

        return output ?? throw new ArgumentException($"No stack with ID {id} exists.");
    }

    public async Task<StackListItem?> GetStackListItemBySortNameAsync(string sortName)
    {
        StackListItem? output = null;

        var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list item by sort name");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetBySortName_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SortName", sortName);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    output.LastStudied = reader.GetDateTime(3);
                }
            }
        }, "get stack list item by sort name");

        await connection.CloseAsync();

        return output;
    }

    public async Task<StackListItem> GetStackListItemByFlashcardIdAsync(int flashcardId)
    {
        StackListItem? output = null;

        var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get stack list item by flashcard id");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_GetByFlashcardId_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new StackListItem
                {
                    Id = reader.GetInt32(0),
                    ViewName = reader.GetString(1),
                    Cards = reader.GetInt32(2)
                };
                if (!reader.IsDBNull(3))
                {
                    output.LastStudied = reader.GetDateTime(3);
                }
            }
        }, "get stack list item by flashcard id");

        await connection.CloseAsync();

        return output ?? throw new ArgumentException($"No stack with flashcard ID {flashcardId} exists.");
    }

    public async Task CreateStackAsync(NewStack stack)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "create stack");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_Create_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ViewName", stack.ViewName);
        cmd.Parameters.AddWithValue("@SortName", stack.SortName);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "create stack");

        await connection.CloseAsync();
    }

    public async Task DeleteStackAsync(int stackId)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "delete stack");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_Delete_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", stackId);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "delete stack");

        await connection.CloseAsync();
    }

    public async Task RenameStackAsync(int oldStackId, NewStack updatedStack)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "rename stack");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Stack_Rename_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", oldStackId);
        cmd.Parameters.AddWithValue("@ViewName", updatedStack.ViewName);
        cmd.Parameters.AddWithValue("@SortName", updatedStack.SortName);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "rename stack");

        await connection.CloseAsync();
    }

    public async Task<int> CountFlashcardsAsync(int stackId)
    {
        var output = 0;

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "count flashcards in a stack");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_Count_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", stackId);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = reader.GetInt32(0);
            }
        }, "count flashcards in a stack");

        await connection.CloseAsync();

        return output;
    }

    public async Task<bool> CardInStack(int stackId, int flashcardId)
    {
        var output = false;

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "check if card is in stack");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_IsInStack_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", stackId);
        cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
        await TryOrDieAsync(async () =>
        {
            var result = await cmd.ExecuteScalarAsync() ?? 0;
            output = ((int)result) != 0;
        }, "check if card is in stack");

        await connection.CloseAsync();

        return output;
    }

    public async Task<List<ExistingFlashcard>> GetFlashcardListAsync(int stackId, int? take = null, int skip = 0)
    {
        var output = new List<ExistingFlashcard>();

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get flashcard list");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_GetMultiple_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", stackId);
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                output.Add(new ExistingFlashcard
                {
                    Id = reader.GetInt32(0),
                    Front = reader.GetString(1),
                    Back = reader.GetString(2)
                });
            }
        }, "get flashcard list");

        await connection.CloseAsync();

        return output;
    }

    public async Task<ExistingFlashcard> GetFlashcardByIdAsync(int id)
    {
        ExistingFlashcard? output = null;
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get flashcard by id");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_GetById_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FlashcardId", id);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = new ExistingFlashcard
                {
                    Id = reader.GetInt32(0),
                    Front = reader.GetString(1),
                    Back = reader.GetString(2)
                };
            }
        }, "get flashcard by id");

        await connection.CloseAsync();

        return output ?? throw new ArgumentException($"No flashcard with ID {id} exists.");
    }

    public async Task CreateFlashcardAsync(NewFlashcard flashcard)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "create flashcard");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_Create_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@StackId", flashcard.StackId);
        cmd.Parameters.AddWithValue("@Front", flashcard.Front);
        cmd.Parameters.AddWithValue("@Back", flashcard.Back);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "create flashcard");

        await connection.CloseAsync();
    }

    public async Task UpdateFlashcardAsync(ExistingFlashcard flashcard)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "update flashcard");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_Update_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FlashcardId", flashcard.Id);
        cmd.Parameters.AddWithValue("@Front", flashcard.Front);
        cmd.Parameters.AddWithValue("@Back", flashcard.Back);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "update flashcard");

        await connection.CloseAsync();
    }

    public async Task MoveFlashcardAsync(int flashcardId, int newStackId)
    {
        // Care must be taken to keep the history correct. Thus, a new history row must be created with the same study date as the old one.
        // But first we check for any existing history rows for the new stack with the same date and time. If there are any, we use that one instead.
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "move flashcard");

        var transaction = connection.BeginTransaction();

        try
        {
            List<HistoryRow> oldHistoryRows = new();

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "dbo.History_GetMultipleByFlashcard_tr";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
                cmd.Transaction = transaction;
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    oldHistoryRows.Add(new HistoryRow
                    {
                        HistoryId = reader.GetInt32(0),
                        StackId = reader.GetInt32(1),
                        StartedAt = reader.GetDateTime(2)
                    });
                }
            }

            foreach (var historyRow in oldHistoryRows)
            {
                int newHistoryId = -1;

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "dbo.History_GetByStackAndDateOrCreate_tr";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StackId", newStackId);
                    cmd.Parameters.AddWithValue("@StartedAt", historyRow.StartedAt);
                    cmd.Transaction = transaction;
                    int scopeIdentity = (int)(await cmd.ExecuteScalarAsync() ?? throw new ApplicationException("no new history id returned"));
                    newHistoryId = (int)scopeIdentity;
                }

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "dbo.StudyResult_MoveMultiple_tr";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
                    cmd.Parameters.AddWithValue("@OldHistoryId", historyRow.HistoryId);
                    cmd.Parameters.AddWithValue("@NewHistoryId", newHistoryId);
                    cmd.Transaction = transaction;
                    await cmd.ExecuteNonQueryAsync();
                }

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "dbo.History_DeleteUnused_tr";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@HistoryId", historyRow.HistoryId);
                    cmd.Transaction = transaction;
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "dbo.Flashcard_MoveStack_tr";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FlashcardId", flashcardId);
                cmd.Parameters.AddWithValue("@StackId", newStackId);
                cmd.Transaction = transaction;
                await cmd.ExecuteNonQueryAsync();
            }

            transaction.Commit();
        }
        catch (Exception ex) when (ex is SqlException || ex is ApplicationException)
        {
            transaction.Rollback();
            Console.Clear();
            Console.WriteLine($"Failed to move flashcard: {ex.Message}\nAborting!");
            Environment.Exit(1);
        }

        await connection.CloseAsync();
    }

    public async Task DeleteFlashcardAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "delete flashcard");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.Flashcard_Delete_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FlashcardId", id);
        await TryOrDieAsync(cmd.ExecuteNonQueryAsync, "delete flashcard");

        await connection.CloseAsync();
    }

    public async Task<int> CountHistoryAsync()
    {
        var output = 0;

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "count history");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.History_Count_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                output = reader.GetInt32(0);
            }
        }, "count history");

        await connection.CloseAsync();

        return output;
    }

    public async Task<List<HistoryListItem>> GetHistoryListAsync(int? take = null, int skip = 0)
    {
        var output = new List<HistoryListItem>();

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get history list");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.History_GetMultiple_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                output.Add(new HistoryListItem
                {
                    Id = reader.GetInt32(0),
                    StartedAt = reader.GetDateTime(1),
                    StackViewName = reader.GetString(2),
                    CardsStudied = reader.GetInt32(3),
                    CorrectAnswers = reader.GetInt32(4)
                });
            }
        }, "get history list");

        await connection.CloseAsync();

        return output;
    }

    public async Task AddHistoryAsync(NewHistory history)
    {
        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "add history");

        var transaction = connection.BeginTransaction();

        try
        {
            int historyId = -1;
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "dbo.History_Create_tr";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartedAt", history.StartedAt);
                cmd.Parameters.AddWithValue("@StackId", history.StackId);
                cmd.Transaction = transaction;
                decimal scopeIdentity = (decimal)(await cmd.ExecuteScalarAsync() ?? throw new ApplicationException("no new history id returned"));
                historyId = (int)scopeIdentity;
            }

            foreach (NewStudyResult result in history.Results)
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.StudyResult_Create_tr";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HistoryId", historyId);
                cmd.Parameters.AddWithValue("@FlashcardId", result.FlashcardId);
                cmd.Parameters.AddWithValue("@WasCorrect", result.WasCorrect);
                cmd.Parameters.AddWithValue("@AnsweredAt", result.AnsweredAt);
                cmd.Transaction = transaction;
                await cmd.ExecuteNonQueryAsync();
            }

            transaction.Commit();
        }
        catch (Exception ex) when (ex is SqlException || ex is ApplicationException)
        {
            transaction.Rollback();
            Console.Clear();
            Console.WriteLine($"Failed to add history: {ex.Message}\nAborting!");
            Environment.Exit(1);
        }
    }

    public async Task<List<ExistingStudyResult>> GetStudyResults(int historyId, int? take = null, int skip = 0)
    {
        var output = new List<ExistingStudyResult>();

        using var connection = new SqlConnection(_connectionString);
        await TryOrDieAsync(connection.OpenAsync, "get study results");

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "dbo.StudyResult_GetMultiple_tr";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@HistoryId", historyId);
        cmd.Parameters.AddWithValue("@Take", take);
        cmd.Parameters.AddWithValue("@Skip", skip);
        await TryOrDieAsync(async () =>
        {
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                output.Add(new ExistingStudyResult
                {
                    Ordinal = (int)reader.GetInt64(0),
                    Front = reader.GetString(1),
                    AnsweredAt = reader.GetDateTime(2),
                    WasCorrect = reader.GetBoolean(3)
                });
            }
        }, "get study results");

        await connection.CloseAsync();

        return output;
    }

    private static async Task TryOrDieAsync(Func<Task> func, string purpose, string? formatError = null)
    {
        if (formatError == null)
        {
            try
            {
                await func.Invoke();
            }
            catch (SqlException ex)
            {
                Console.Clear();
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
        else
        {
            try
            {
                await func.Invoke();
            }
            catch (SqlException ex)
            {
                Console.Clear();
                Console.WriteLine($"Failed to {purpose}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
            catch (FormatException ex)
            {
                Console.Clear();
                Console.WriteLine($"{formatError}: {ex.Message}\nAborting!");
                Environment.Exit(1);
            }
        }
    }

    private class HistoryRow
    {
        public int HistoryId { get; set; }
        public int StackId { get; set; }
        public DateTime StartedAt { get; set; }
    }
}