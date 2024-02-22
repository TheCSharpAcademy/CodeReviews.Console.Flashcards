using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Flashcards.jkjones98;

internal class FlashcardController
{
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    internal void InsertFlashcardDb(Flashcard flashcard)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            $@"INSERT INTO Flashcards (Front, Back, StackId) VALUES ('{flashcard.Front}', '{flashcard.Back}', {flashcard.StackId})";
        tableCmd.ExecuteNonQuery();
    }

    internal void ViewFlashcardDto(int stackId)
    {
        List<FlashcardDto> flashcardTable = new List<FlashcardDto>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM Flashcards WHERE StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            int cardId = 1;
            while(reader.Read())
            {
                flashcardTable.Add(new FlashcardDto
                    {
                        FlashcardId = cardId, 
                        Front = reader.GetString(1),
                        Back = reader.GetString(2),
                    });
                cardId++;
            }
        }
        else
            Console.WriteLine("\nNo rows found");

        ShowTable.CreateFlashcardTable(flashcardTable);
    }

    internal void ViewFlashcardDb(int stackId, string prmryKey)
    {
        List<FlashcardDto> flashcardTable = new List<FlashcardDto>();
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"SELECT * FROM Flashcards WHERE {prmryKey}={stackId}";
        using var reader = tableCmd.ExecuteReader();
        if(reader.HasRows)
        {
            while(reader.Read())
            {
                flashcardTable.Add(new FlashcardDto
                    {
                        FlashcardId = reader.GetInt32(0), 
                        Front = reader.GetString(1),
                        Back = reader.GetString(2),
                    });
            }
        }
        else
            Console.WriteLine("\nNo rows found");

        ShowTable.CreateFlashcardTable(flashcardTable);
    }

    internal void ChangeFlashcardDb(int cardId, string col, string val, int stackId)
    {   
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            $@"UPDATE Flashcards
                SET {col} = '{val}'
                WHERE FlashId={cardId}
                AND StackId={stackId}";
        tableCmd.ExecuteNonQuery();
    }

    internal void ChangeFlashcardDb(int cardId, string col, int val, int stackId)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            $@"UPDATE Flashcards
                SET {col} = {val}
                WHERE FlashId={cardId}
                AND StackId={stackId}";
        tableCmd.ExecuteNonQuery();
    }

    internal void DeleteFlashcardDb(int cardId)
    {
        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = $"DELETE FROM Flashcards WHERE FlashId={cardId}";
        tableCmd.ExecuteNonQuery();
    }

    internal bool CheckIdExists(int checkId, string table, string tablePk, int stackId)
    {
        bool checkedId = false;

        using var connection = new SqlConnection(connectionString);
        using var tableCmd = connection.CreateCommand();
        connection.Open();
        tableCmd.CommandText = 
            $@"USE FlashcardsDb
            SELECT * FROM {table} WHERE {tablePk}='{checkId}' AND StackId={stackId}";
        using var reader = tableCmd.ExecuteReader();
        

        if(table == "Stacks")
        {
            Stack stackCheck = new();
            if(reader.HasRows)
            {
                reader.Read();
                stackCheck.StackId = reader.GetInt32(0);
                stackCheck.StackName = reader.GetString(1);
                checkedId = true;
            }
            else
            {
                Console.WriteLine("\nNo rows found");
            }
        }
        else if(table == "Flashcards")
        {
            FlashcardDto cardCheck = new();
            if(reader.HasRows)
            {
                reader.Read();
                cardCheck.FlashcardId = reader.GetInt32(0);
                cardCheck.Front = reader.GetString(1);
                cardCheck.Back = reader.GetString(2);
                checkedId = true;
            }
            else
            {
                Console.WriteLine("\nNo rows found");
            }
        }
        return checkedId;
    }

    internal bool CheckIdExists(int checkId, string table, string tablePk)
        {
            bool checkedId = false;

            using var connection = new SqlConnection(connectionString);
            using var tableCmd = connection.CreateCommand();
            connection.Open();
            tableCmd.CommandText = 
                $@"USE FlashcardsDb
                SELECT * FROM {table} WHERE {tablePk}='{checkId}'";
            using var reader = tableCmd.ExecuteReader();
            

            if(table == "Stacks")
            {
                Stack stackCheck = new();
                if(reader.HasRows)
                {
                    reader.Read();
                    stackCheck.StackId = reader.GetInt32(0);
                    stackCheck.StackName = reader.GetString(1);
                    checkedId = true;
                }
                else
                {
                    Console.WriteLine("\nNo rows found");
                }
            }
            else if(table == "Flashcards")
            {
                FlashcardDto cardCheck = new();
                if(reader.HasRows)
                {
                    reader.Read();
                    cardCheck.FlashcardId = reader.GetInt32(0);
                    cardCheck.Front = reader.GetString(1);
                    cardCheck.Back = reader.GetString(2);
                    checkedId = true;
                }
                else
                {
                    Console.WriteLine("\nNo rows found");
                }
            }   
            return checkedId;
        }
}