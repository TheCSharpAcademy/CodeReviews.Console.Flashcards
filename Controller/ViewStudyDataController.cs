
class ViewStudyDataController
{
    StacksDatabaseManager stacksDatabaseManager = new();
    StudySessionDatabaseManager studySessionDatabaseManager = new();
    Stack stack = default;
    public async Task StartAsync()
    {
        stack = GetInput.Selection(await stacksDatabaseManager.GetLogs());
        
        List<StudySession> sessions = 
            await studySessionDatabaseManager.GetLogs(stack);
        
        DisplayData.Table(sessions);
    }
}