using Flashcards.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Repository
{
    public interface IFlashcardRepository
    {
        void AddFlashcard(int stackId, string question, string answer);
        List<FlashcardDTO> GetFlashcardsByStack(int stackId);
        void DeleteFlashcard(int Id);
    }
}
