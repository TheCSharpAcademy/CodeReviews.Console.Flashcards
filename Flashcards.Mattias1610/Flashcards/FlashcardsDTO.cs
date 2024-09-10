using System.Data.SqlClient;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Flashcards
{
    public class Flashcard(){
        public int Id {get; set;}
        public int StackID{get; set;}
        public string Question{get;set;}
        public string Answer{get;set;}
    }
    public class FlashcardsDto
    {
        public string Question{get;set;}
        public string Answer{get;set;}

    }
}
