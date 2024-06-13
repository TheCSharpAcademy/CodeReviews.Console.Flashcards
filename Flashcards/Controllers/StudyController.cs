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

    public List<StudyDto> GetStudySessions(List<Stack> stacks)
    {
        var studySessions = new List<StudyDto>();
        foreach (var item in _studyRepo.GetStudySessions())
        {
            var session = new StudyDto
            {
                Id = item.Id,
                StackName = stacks.First(x => x.Id == item.StackId).Name,
                Date = item.Date,
                PercentageScore = item.PercentageScore
            };
            studySessions.Add(session);
        }

        return studySessions;
    }

    public List<MonthlySessionsPivot> GetMonthlyReports(int year, List<Stack> stacks)
    {
        var monthlySessions = _studyRepo.GetMonthlyReports(year);

        foreach (var session in monthlySessions)
        {
            var stack = stacks.FirstOrDefault(s => s.Id == session.StackId);
            if (stack != null)
            {
                session.StackName = stack.Name;
            }
        }
        return monthlySessions;
    }
}
