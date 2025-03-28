namespace FlashCards
{
    internal class StudySessionRepositoryService
    {
        public StudySessionRepository StudySessionRepository { get; set; }

        public StudySessionRepositoryService(StudySessionRepository repository)
        {
            StudySessionRepository = repository;
        }
        public void PrepareRepository(List<CardStack> stacks, List<StudySession> defaultData)
        {

            if (!StudySessionRepository.DoesTableExist())
            {
                StudySessionRepository.CreateTable();
                StudySessionRepository.AutoFill(stacks, defaultData);
            }
        }
    }
}
