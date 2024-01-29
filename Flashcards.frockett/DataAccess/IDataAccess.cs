
using Library.Models;

namespace DataAccess;

public interface IDataAccess
{
    public void InitDatabase();
    //public string GetConnectionString();
    public void InsertCard(CardModel flashcard);
    public void DeleteCardById(int cardId);
    public void InsertStack(StackModel stack);
    public void DeleteStackById(int stackId);
    public int CheckForStackContents(int stackId);
    public List<StackModel> GetListOfStacks();
    public List<CardModel> GetCardsByStackId(int id);
    public StackModel GetStackById(int stackId);
    public int InsertStudySession(StudySessionModel studySession);
    public List<StudySessionModel> GetStudySessions();
}
