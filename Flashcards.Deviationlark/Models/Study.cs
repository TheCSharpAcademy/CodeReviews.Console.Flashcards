using System.Data.Common;

namespace Flashcards
{
    class StudyModel
    {
        public int Id { get; set; }

        public string? Date { get; set; }

        public int Score { get; set; }
    }
}