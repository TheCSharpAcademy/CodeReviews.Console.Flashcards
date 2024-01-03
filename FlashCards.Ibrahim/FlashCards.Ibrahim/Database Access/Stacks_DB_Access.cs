using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Ibrahim.Models;

namespace FlashCards.Ibrahim.Database_Access
{
    internal class Stacks_DB_Access
    {
        string _connectionString;
        public Stacks_DB_Access(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public static void Insert_Stack(string connectionString)
        {

        }
        public static void Update_Stack(string connectionString)
        {
        }
        public static void Delete_Stack(string connectionString)
        {

        }
        public static Stacks Get_Stack(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
