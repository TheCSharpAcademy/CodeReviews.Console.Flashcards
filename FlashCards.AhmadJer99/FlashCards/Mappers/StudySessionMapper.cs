using FlashCards.Models;
using FlashCards.Dtos;

namespace FlashCards.Mappers;

internal static class StudySessionMapper
{
    public static StudySessionDto ToStudySessionDto(this StudySession studySession)
    {
        return new StudySessionDto()
        {
            FormattedSessionDate = studySession.session_date.ToString("yyyy/MM/dd"),
            score = studySession.score,
        };
    }
}
