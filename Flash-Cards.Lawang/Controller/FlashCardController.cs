using Dapper;
using Flash_Cards.Lawang.Models;
using Microsoft.Data.SqlClient;

namespace Flash_Cards.Lawang.Controller;

public class FlashCardController
{
    private readonly string _connectionString;
    public FlashCardController(string cs)
    {
        _connectionString = cs;
    }
    public void CreateFlashCardTable()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string createSQL =
                @"IF NOT EXISTS
                    (
                    SELECT * FROM sys.tables
                    WHERE name = 'flashcards' AND schema_id = SCHEMA_ID('dbo')
                    )
                BEGIN
                    CREATE TABLE flashcards
                    (Id INT PRIMARY KEY IDENTITY(1,1),
                    Front VARCHAR(50) NOT NULL,
                    Back VARCHAR(50) NOT NULL,
                    StackId INT,
                    FOREIGN KEY(StackId) REFERENCES stacks(Id) ON DELETE CASCADE
                    );
                END";

            connection.Execute(createSQL);
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public List<FlashCard> GetAllFlashCard(Option? chosenStack = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string getAllSQL;

            if (chosenStack != null)
            {
                getAllSQL = $@"SELECT * FROM flashcards WHERE StackId = {chosenStack.Value}";
            }
            else
            {
                getAllSQL = @"SELECT * FROM flashcards";
            }

            return connection.Query<FlashCard>(getAllSQL).ToList();
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return new List<FlashCard>();
    }

    public int CreateFlashCard(FlashCardDTO flashCardDTO, int Id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string createSQL =
                @"INSERT INTO flashcards
                (Front, Back, StackId)
                VALUES (@front, @back, @id)";

            var param = new { @front = flashCardDTO.Front, @back = flashCardDTO.Back, @id = Id };

            return connection.Execute(createSQL, param);
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return 0;
    }

    public void SeedValueForTesting()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string createSQL = 
                @"
                IF NOT EXISTS(SELECT 1 FROM flashcards)
                BEGIN
                    INSERT INTO flashcards
                        (Front, Back, StackId)
                    VALUES 
                        ('Hola', 'Hello', 1),
                        ('Adios', 'Bye', 1),
                        ('Manzana', 'Apple', 1),
                        ('Gata', 'Cat', 1),
                        ('Bonjour', 'Hello', 2),
                        ('Au revoir', 'Bye', 2),
                        ('Pomme', 'Apple', 2),
                        ('Chatte', 'Cat', 2),
                        ('Ciao', 'Hello', 3),
                        ('Arrivederci', 'Good Bye', 3),
                        ('Mela', 'Apple', 3),
                        ('Gatta', 'Cat', 3)
                END";
                
                connection.Execute(createSQL);
        }
        catch(SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public int UpdateFlashCard(FlashCardDTO flashCardDTO)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string updateSQL =
                @"UPDATE flashcards
                SET Front = @front,
                Back = @back
                WHERE Id = @id";

            var param = new { @id = flashCardDTO.Id, @front = flashCardDTO.Front, @back = flashCardDTO.Back };

            return connection.Execute(updateSQL, param);

        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return 0;
    }

    public int DeleteFlashCard(FlashCardDTO flashCardDTO)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string deleteSQL =
                @"DELETE FROM flashcards
                WHERE Id = @id";

            var param = new { @id = flashCardDTO.Id };

            return connection.Execute(deleteSQL, param);
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return 0;
    }
}
