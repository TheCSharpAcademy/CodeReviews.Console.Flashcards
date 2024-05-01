using Flashcards.Database;
using Flashcards.Services;
using Flashcards.DTO;
using Flashcards.Models;
using Dapper;

namespace Flashcards.DAO;

public class FlashCardDao
{
    private readonly DatabaseContext _dbContext;

    public FlashCardDao(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void InsertNewFlashCard(FlashCardDto flashCardDto, StackDto stack)
    {
        try
        {
            FlashCard flashCard = new FlashCard
            {
                StackID = stack.StackID,
                Front = flashCardDto.Front,
                Back = flashCardDto.Back
            };

            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"INSERT INTO {ConfigSettings.tbFlashCardsName} (StackID, Front, Back) VALUES (@StackID, @Front, @Back)";
                dbConnection.Execute(sql, new { flashCard.StackID, flashCard.Front, flashCard.Back });
            }
        }
        catch
        {
            throw;
        }
    }

    public IEnumerable<FlashCardDto> GetAllFlashCardsByStackId(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"SELECT CardID, Front, Back FROM {ConfigSettings.tbFlashCardsName} WHERE StackID = @StackID";
                return dbConnection.Query<FlashCardDto>(sql, new { stack.StackID });
            }
        }
        catch
        {
            throw;
        }
    }

    public bool DeleteFlashCard(FlashCardDto flashCard)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"DELETE FROM {ConfigSettings.tbFlashCardsName} WHERE CardID = @CardID";
                int rowsAffected = dbConnection.Execute(sql, new { flashCard.CardID });

                return rowsAffected > 0;
            }
        }
        catch
        {
            throw;
        }
    }

    public bool UpdateFlashCard(FlashCardDto flashCard)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"UPDATE {ConfigSettings.tbFlashCardsName} SET Front = @Front, Back = @Back WHERE CardID = @CardID";
                int rowsAffected = dbConnection.Execute(sql, new { flashCard.Front, flashCard.Back, flashCard.CardID });

                return rowsAffected > 0;
            }
        }
        catch
        {
            throw;
        }
    }

    public IEnumerable<FlashCardDto> GetAllFlashCards()
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"SELECT CardID, Front, Back FROM {ConfigSettings.tbFlashCardsName}";
                return dbConnection.Query<FlashCardDto>(sql);
            }
        }
        catch
        {
            throw;
        }
    }   
}
