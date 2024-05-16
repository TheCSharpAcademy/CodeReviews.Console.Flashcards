using Flashcards.Database;
using Flashcards.Models;
using Dapper;
using Flashcards.Services;

namespace Flashcards.DAO;

public class DatabaseSeederDao
{
    private readonly DatabaseContext _dbContext;

    public DatabaseSeederDao(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void InsertStacksAndFlashCards(List<Stack> stacks, Dictionary<string, List<FlashCard>> flashCards)
    {
        using (var transaction = _dbContext.GetConnectionToFlashCards().BeginTransaction())
        {
            try
            {
                foreach (var stack in stacks)
                {
                        
                    var stackId = transaction.Connection.Query<int>(@$"
                        INSERT INTO {ConfigSettings.TableNameStack} (StackName) VALUES (@StackName);
                        SELECT CAST(SCOPE_IDENTITY() as int);",
                        new { stack.StackName },
                        transaction).Single();

                    foreach (var card in flashCards[stack.StackName])
                    {
                        card.StackID = stackId;
                        transaction.Connection.Execute(@$"
                            INSERT INTO {ConfigSettings.TableNameFlashCards} (Front, Back, StackID) VALUES (@Front, @Back, @StackID);",
                            card,
                            transaction);
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}