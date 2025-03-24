using Dapper;
using Flashcards.Models.Dtos;
using Flashcards.Models.utils;
using Npgsql;

namespace Flashcards.Models;

public class FlashcardsRepository
{
    public void DeleteByStackId(int stackId)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"DELETE FROM {Context.FlashcardsTable}
                       WHERE stack_id=@StackId";

                connection.Execute(tableCommand, new { StackId = stackId });
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void DeleteById(int id)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"DELETE FROM {Context.FlashcardsTable}
                       WHERE id=@Id";

                connection.Execute(tableCommand, new { Id = id });
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void Update(FlashcardDto flashcard)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"UPDATE {Context.FlashcardsTable} SET front_text=@FrontText, back_text=@BackText WHERE id=@Id";

                connection.Execute(tableCommand, flashcard);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public void Insert(FlashcardDto flashcard)
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                string tableCommand =
                    @$"INSERT INTO {Context.FlashcardsTable} (stack_id, front_text, back_text)
                       VALUES (@StackId, @FrontText, @BackText)";

                connection.Execute(tableCommand, flashcard);
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }

    public FlashcardDto GetRandomFlashcardFromStack(int stackId)
    {
        FlashcardDto flashcard = new();

        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                flashcard = connection.QuerySingle<FlashcardDto>(
                    @$"SELECT Id, stack_id AS StackId, front_text AS FrontText, back_text AS BackText
                        FROM {Context.FlashcardsTable}
                        WHERE stack_id='{stackId}'
                        ORDER BY RANDOM() LIMIT 1");
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }

        return flashcard;
    }

    public List<FlashcardDto> GetFlashcardsByStack(int stackId)
    {
        List<FlashcardDto> flashcards = [];

        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();

                flashcards = connection
                    .Query<FlashcardDto>(
                        @$"SELECT Id, stack_id AS StackId, front_text AS FrontText, back_text AS BackText FROM {Context.FlashcardsTable}
                           WHERE stack_id='{stackId}'
                           ORDER BY id")
                    .ToList();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }

        return flashcards;
    }
}