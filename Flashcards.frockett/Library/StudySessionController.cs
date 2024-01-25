using DataAccess;

namespace Library;

public class StudySessionController
{
    private readonly IDataAccess dataAccess;
    
    public StudySessionController(IDataAccess dataAccess)
    {
        this.dataAccess = dataAccess;
    }
}
