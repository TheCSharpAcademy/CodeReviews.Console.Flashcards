using System;
using System.Data.Common;
using System.Data.SqlClient;
using Flashcards;

class Program
{
    static void Main(string[] args){
        
        DatabaseManager db = new DatabaseManager();
        db.Menu();
    }    
}