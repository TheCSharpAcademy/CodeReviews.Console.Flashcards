using FlashcardsProgram.Database;

namespace FlashcardsProgram.StudySession;

public class StudySessionsRepository(string tableName) : BaseRepository<StudySessionDAO>(tableName)
{
}