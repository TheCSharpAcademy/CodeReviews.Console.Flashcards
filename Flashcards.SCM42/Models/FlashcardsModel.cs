using System.Configuration;
using System.Runtime.InteropServices;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards;

public class FlashcardsModel
{
    static string connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;

    internal static List<Flashcard> FetchCardsInStack(int stackId)
    {
        var cardsList = new List<Flashcard>();
       
        var parameters = new { StackId = stackId };
        var sqlCommand = $@"SELECT CardId, 
                                   StackId, 
                                   Front, 
                                   Back,
                            ROW_NUMBER() OVER (ORDER BY CardId) AS SequentialId
                            FROM Flashcards
                            WHERE StackId = @StackId";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(sqlCommand, parameters);

                while (reader.Read())
                {
                    cardsList.Add(
                    new Flashcard
                    {
                        CardId = reader.GetInt32(0),
                        StackId = reader.GetInt32(1),
                        CardFront = reader.GetString(2),
                        CardBack = reader.GetString(3),
                        RowNumber = reader.GetInt64(4),
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return cardsList;
    }

    internal static List<Flashcard> FetchXAmountCard(int stackId, int amount)
    {
        var cardList = new List<Flashcard>();

        var parameters = new { StackId = stackId }; 
        var sqlCommand = $@"SELECT TOP {amount} CardId, StackId, Front, Back,
                            ROW_NUMBER() OVER (ORDER BY CardId) AS SequentialId
                            FROM Flashcards
                            WHERE StackId = @StackId";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(sqlCommand, parameters);

                while (reader.Read())
                {
                    cardList.Add(
                    new Flashcard
                    {
                        CardId = reader.GetInt32(0),
                        StackId = reader.GetInt32(1),
                        CardFront = reader.GetString(2),
                        CardBack = reader.GetString(3),
                        RowNumber = reader.GetInt64(4),
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return cardList;
    }

    internal static void InsertCard(string cardFront, string cardBack, int stackId)
    {
        var parameters = new { Front = cardFront, Back = cardBack, StackId = stackId };
        var sqlCommand = $@"INSERT INTO Flashcards (Front, Back, StackId)
                            VALUES (@Front, @Back, @StackId)";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }

    internal static void UpdateCard(int cardId, string cardSide, string? newValue)
    {
        var parameters = new { CardId = cardId, NewValue = newValue };
        var sqlCommand = $@"UPDATE Flashcards
                            SET {cardSide} = @NewValue
                            WHERE CardId = @CardId";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }

    internal static void DeleteCard(int cardNumber)
    {
        var parameters = new { CardNumber = cardNumber }; 
        var sqlCommand = $@"DELETE FROM Flashcards
                            WHERE CardId = @CardNumber";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }

    internal static int GetCardQuantity(int stackId)
    {
        int count = 0;

        var parameters = new { StackId = stackId };
        var sqlCommand = $@"SELECT COUNT(*) 
                            FROM Flashcards
                            WHERE StackId = @StackId";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                count = (int)connection.ExecuteScalar(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return count;
    }

    // Methods for Stacks
    internal static List<Stack> FetchStacks()
    {
        var sqlCommand = $@"SELECT StackId,
                                   StackName,
                                   CardQuantity,
                            ROW_NUMBER() OVER (ORDER BY StackId) AS SequentialId                              
                            FROM Stacks";
        var stackList = new List<Stack>();

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(sqlCommand);

                while (reader.Read())
                {
                    stackList.Add(
                    new Stack
                    {
                        StackId = reader.GetInt32(0),
                        StackName = reader.GetString(1),
                        CardQuantity = reader.GetInt32(2),
                        RowNumber = reader.GetInt64(3)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return stackList;
    }

    internal static int InsertStack(string stackName)
    {
        int stackId = -1;

        var parameters = new { StackName = stackName };
        var sqlCommand = $@"INSERT INTO Stacks (StackName, CardQuantity)
                            OUTPUT INSERTED.StackId 
                            VALUES (@StackName, 0)";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                stackId = (int)connection.ExecuteScalar(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }

        return stackId;
    }

    internal static void DeleteStack(string? stackName)
    {
        var parameters = new { StackName = stackName };
        var sqlCommand = $@"DELETE FROM Stacks
                            WHERE StackName = @StackName";                                              

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }

    internal static void UpdateCardQuantity(int count, int stackId)
    {
        var parameters = new { Count = count, StackId = stackId };
        var sqlCommand = $@"UPDATE Stacks
                            SET CardQuantity = @Count
                            WHERE StackId = @StackId";

        using (var connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sqlCommand, parameters);
            }
            catch (Exception ex)
            {
                Views.ShowErrorMessage(ex.Message);
            }
        }
    }
}

public class Flashcard
{
    public int CardId { get; set; }
    public int StackId { get; set; }
    public string? CardFront { get; set; }
    public string? CardBack { get; set; }
    public long RowNumber { get; set; }
}

public class FlashcardDTO
{
    public string? CardFront { get; set; }
    public string? CardBack { get; set; }
    public long RowNumber { get; set; }
}

public class Stack
{
    public int StackId { get; set; }
    public string? StackName { get; set; }
    public int CardQuantity { get; set; }
    public long RowNumber { get; set; }
}

public class StackDTO
{
    public string? StackName { get; set; }
    public int CardQuantity { get; set; }
    public long RowNumber { get; set; }
}
