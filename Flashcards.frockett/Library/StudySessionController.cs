using DataAccess;

namespace Library;

public class StudySessionController
{
    private readonly IDataAccess dataAccess;
    private readonly InputValidation inputValidation;
    
    public StudySessionController(IDataAccess dataAccess, InputValidation inputValidation)
    {
        this.dataAccess = dataAccess;
        this.inputValidation = inputValidation;
    }
}
