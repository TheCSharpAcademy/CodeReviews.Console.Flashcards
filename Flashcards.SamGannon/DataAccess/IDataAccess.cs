using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IDataAccess
    {
        // Table Creations
        public void CreateStackTable();
        public void CreateFlashcardTable();
        public void CreateStudyTable();

        // stack services
        bool CheckStackName(string formattedStackName);
        void AddStack(string? stackName);
        public List<StackModel> GetAllStacks();
        List<StackModel> GetListOfStackNames();
        void EditStackname(string oldStackName, string? newStackName);
        StackModel GetStackByName(string stackName);
        void DeleteStack(string stackId);
        public int GetStackId(string stackName);

        // flashcard services
        List<Flashcard> GetFlashcardsByStackId(int stackId);
        void AddFlashcard(string? question, string? answer, int stackId);
        void DeleteFlashcard(int flashcardId, string stackName);
        List<Flashcard> GetAllFlashcards(string stackName);
        Flashcard GetFlashcardsById(int flashcardId);

        // Study Session
        void InsertStudySession(int stackId, DateTime startTime, DateTime endTime, double score);
    }
}
