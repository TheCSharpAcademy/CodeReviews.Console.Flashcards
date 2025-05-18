using Flashcards.Models;
using Flashcards.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Repository
{
    public interface IStudySessionRepository
    {
        void SaveStudySession(StudySession session);
        List<StudySessionDTO> GetAllSessions();
    }
}
