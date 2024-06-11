﻿using Dapper;

public class FlashcardRepository : IHandleFlashcards
{
    private DatabaseManager _databaseManager;

    public FlashcardRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void CreateFlashcard(Flashcard flashcard)
    {
        using (var conn = _databaseManager.GetConnection())
        {
            var query = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
            conn.Execute(query, flashcard);
        }
    }

    public void DeleteFlashcard(int flashcardId)
    {
        throw new NotImplementedException();
    }

    public List<Flashcard> GetFlashcardsByStack(int stackId)
    {
        throw new NotImplementedException();
    }
}