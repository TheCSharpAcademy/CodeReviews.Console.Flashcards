public class StudyController
{
    private StudyRepository _studyRepo;

    public StudyController(StudyRepository studyRepo)
    {
        _studyRepo = studyRepo;
    }

    public void AddStudy(StudySession studySessions)
    {
        if (studySessions.TotalQuestions > 0)
        {
            _studyRepo.AddStudy(studySessions);
        }
        else
        {
            Console.WriteLine("Ran into a error adding study session to DB.");
        }

        
    }
}
