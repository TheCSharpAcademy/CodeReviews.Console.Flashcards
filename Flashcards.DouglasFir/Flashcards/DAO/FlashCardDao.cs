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
                string sql = $"INSERT INTO tb_FlashCards (StackID, Front, Back) VALUES (@StackID, @Front, @Back)";
                dbConnection.Execute(sql, new {flashCard.StackID, flashCard.Front, flashCard.Back });
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to insert new flash card.", ex.Message);
            throw;
        }
    }

    public IEnumerable<FlashCardDto> GetAllFlashCardsByStackId(StackDto stack)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"SELECT CardID, Front, Back FROM tb_FlashCards WHERE StackID = @StackID";
                return dbConnection.Query<FlashCardDto>(sql, new { stack.StackID });
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to retrieve flash cards.", ex.Message);
            throw;
        }
    }

    public bool DeleteFlashCard(FlashCardDto flashCard)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"DELETE FROM tb_FlashCards WHERE CardID = @CardID";
                int rowsAffected = dbConnection.Execute(sql, new { flashCard.CardID });

                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to delete flash card.", ex.Message);
            return false;
        }
    }

    public bool UpdateFlashCard(FlashCardDto flashCard)
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"UPDATE tb_FlashCards SET Front = @Front, Back = @Back WHERE CardID = @CardID";
                int rowsAffected = dbConnection.Execute(sql, new { flashCard.Front, flashCard.Back, flashCard.CardID });

                return rowsAffected > 0;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to update flash card.", ex.Message);
            return false;
        }
    }

    public IEnumerable<FlashCardDto>? GetAllFlashCards()
    {
        try
        {
            using (var dbConnection = _dbContext.GetConnectionToFlashCards())
            {
                string sql = $"SELECT CardID, Front, Back FROM tb_FlashCards";
                return dbConnection.Query<FlashCardDto>(sql);
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Unable to retrieve flash card.", ex.Message);
            return null;
        }
    }   
}
