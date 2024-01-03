using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Ibrahim.Models;

namespace FlashCards.Ibrahim.Database_Acess
{
    internal class Flashcard_DB_Access
    {
        string _connectionString;
        public Flashcard_DB_Access(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static void Insert_Flashcard(string connectionString)
        {

        }
        public static void Update_Flashcard(string connectionString)
        {
        }
        public static void Delete_Flashcard(string connectionString)
        {

        }
        public static Stacks Get_Flashcard(string connectionString)
        {
            throw new NotImplementedException();
        }

    }
}
