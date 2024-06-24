using Dapper;
using FlashcardsProgram.Database;
using FlashcardsProgram.Stacks;

namespace FlashcardsProgram.StudySession;

public class StudySessionsRepository(string tableName) : BaseRepository<StudySessionDAO>(tableName)
{
}