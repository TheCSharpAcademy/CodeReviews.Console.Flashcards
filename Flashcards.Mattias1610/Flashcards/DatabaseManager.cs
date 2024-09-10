using System.Data.SqlClient;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Flashcards
{
    public class DatabaseManager
    {
        string connectionString="Server=localhost;Database=FLASHCARDS;User ID=SA;Password=Password123;";

        public void Menu(){
            bool isRunning = true;

            while(isRunning){
                Console.Clear();
                Console.WriteLine("\t\t-----WELCOME TO MY FLASHCARDS APP-----");
                Console.WriteLine(" TYPE 1 TO MANAGE STACKS");
                Console.WriteLine(" TYPE 2 TO MANAGE FLASHCARDS");
                Console.WriteLine(" TYPE 3 TO SEE FLASHCARDS");
                Console.WriteLine(" TYPE 4 TO START A STUDY SESSION");
                Console.WriteLine(" TYPE 5 TO SEE A STUDY REPORT");
                Console.WriteLine(" TYPE 0 TO EXIT");

                string choice = Console.ReadLine();


                switch(choice){
                    case "1":
                        StacksManager();
                        break;
                    case "2":
                        FlashcardsManager();
                        break;
                    case "3":
                        ShowFlashcards();
                        break;
                    case "4":
                        StudySession();
                        break;
                    
                    case "5":
                        Report();
                        break;
                    case "0":
                        Environment.Exit(0);
                        isRunning = false;
                        break;
                }
            }
        }

        public void Report()
        {
            ReportApp rp = new ReportApp();
            rp.ShowYearlyReport();
        }

        public void StudySession()
        {
            StudyApp st = new StudyApp();
            st.StudyMenu();
        }

        public void ShowFlashcards()
        {
            Console.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var table = connection.Query("SELECT * FROM dbo.STACKS ORDER BY StackID");

                    foreach (var row in table)
                    {
                        Console.WriteLine($"StackID: {row.StackID}\t Name: {row.StackName}\t");
                    }
                }
            FlashcardsApp fs = new FlashcardsApp();
            Console.WriteLine("What stack of flashcards you want to see?");
            int choice = Convert.ToInt32(Console.ReadLine());
            var flashcards = fs.GetFlashcardsByStack(choice);
            
    
            if (flashcards.Any())
            {
                Console.WriteLine("Here are the flashcards from the selected stack:");
                
                
                foreach (var flashcard in flashcards)
                {
                    Console.WriteLine($"Question: {flashcard.Question}, Answer: {flashcard.Answer}");
                }

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("No flashcards found in the selected stack.");
            }
        }

        public void FlashcardsManager()
        {
           using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var table = connection.Query("SELECT * FROM dbo.FLASHCARD");

                    foreach (var row in table)
                    {
                        Console.WriteLine($"FlashcardID: {row.FlashcardID}\t StackID: {row.StackID}\t Question: {row.Question}\t Answer:{row.Answer}\t");
                    }

                    Console.WriteLine("\nWHAT DO YOU WANT TO DO?");
                    Console.WriteLine("TYPE 1 TO INSERT FLASHCARD");
                    Console.WriteLine("TYPE 2 TO DELETE FLASHCARD");
                    Console.WriteLine("TYPE 0 TO GO BACK TO MAIN MENU");
                    string stackChoice = Console.ReadLine();

                    switch(stackChoice){
                        case "1":
                            InsertFlashcard();
                            break;
                        case "2":
                            DeleteFlashcard();
                            break;
                        case "0":
                            Menu();
                            break;
                        default:
                            Console.WriteLine("ERROR");
                            break;
                    }
                }
        }

        public void DeleteFlashcard()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Which row do you want to delete? Enter FlashcardID number");
                    string choice = Console.ReadLine();

                    if(int.TryParse(choice, out int FlashcardID)){
                        string query = "DELETE FROM dbo.FLASHCARD WHERE FlashcardID = @FlashcardID";

                        int rowsAffected = connection.Execute(query, new { FlashcardId = FlashcardID });

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Stack with ID {FlashcardID} deleted successfully.");
                            ResequenceStacks(connection);
                        }
                        else
                        {
                            Console.WriteLine($"No stack found with ID {FlashcardID}.");
                        }
                    }

                    else{
                        Console.WriteLine("ERROR");
                    }

                }
        }

        public void InsertFlashcard()
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        var tableStacks = connection.Query("SELECT * FROM dbo.STACKS ORDER BY StackID");
        
        foreach (var row in tableStacks)
        {
            Console.WriteLine($"StackID: {row.StackID}\t Name: {row.StackName}\t");
        }

        Console.WriteLine("ENTER THE STACK ID:");
        if (!int.TryParse(Console.ReadLine(), out int stackId))
        {
            Console.WriteLine("Invalid Stack ID. Please enter a valid number.");
            return;
        }

        Console.WriteLine("ENTER THE QUESTION:");
        string question = Console.ReadLine();

        Console.WriteLine("ENTER THE ANSWER:");
        string answer = Console.ReadLine();

        string query = @"INSERT INTO dbo.FLASHCARD (StackID, Question, Answer) VALUES ( @StackId, @Question, @Answer)";

        var parameters = new { StackId = stackId, Question = question, Answer = answer };

        connection.Execute(query, parameters);
        Console.WriteLine("Flashcard inserted successfully!");

        Console.WriteLine("Press any key to return to the Flashcards Manager...");
        Console.ReadKey();
        FlashcardsManager();
    }
}


        public void StacksManager()
        {
            Console.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var table = connection.Query("SELECT * FROM dbo.STACKS ORDER BY StackID");

                    foreach (var row in table)
                    {
                        Console.WriteLine($"StackID: {row.StackID}\t Name: {row.StackName}\t");
                    }

                    Console.WriteLine("\nWHAT DO YOU WANT TO DO?");
                    Console.WriteLine("TYPE 1 TO INSERT STACKS");
                    Console.WriteLine("TYPE 2 TO DELETE STACKS");
                    Console.WriteLine("TYPE 3 TO UPDATE STACKS");
                    Console.WriteLine("TYPE 0 TO GO BACK");
                    string stackChoice = Console.ReadLine();

                    switch(stackChoice){
                        case "1":
                            InsertStack();
                            break;
                        case "2":
                            DeleteStack();
                            break;
                        case "3":
                            UpdateStack();
                            break;
                        case "0":
                            Menu();
                            break;
                        default:
                            Console.WriteLine("ERROR");
                            break;
                    }
                }
        }

        public void UpdateStack()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    Console.WriteLine("ENTER THE STACK ID YOU WANT TO UPDATE");
                    string initialStackID = Console.ReadLine();
                    Console.WriteLine("ENTER THE UPDATED STACK NAME");
                    string initialStackName = Console.ReadLine();

                    string query = @$"UPDATE dbo.STACKS SET StackName = @StackName WHERE StackID = @StackId";

                    var parameters = new {StackId = int.Parse(initialStackID), StackName = initialStackName};

                    connection.Execute(query, parameters);

                    StacksManager();
                }
        }

        public void InsertStack()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    

                    Console.WriteLine("ENTER THE STACK ID");
                    string initialStackID = Console.ReadLine();
                    Console.WriteLine("ENTER THE STACK NAME");
                    string initialStackName = Console.ReadLine();

                    string query = @$"INSERT INTO dbo.STACKS (StackID, StackName) VALUES ( @StackID, @StackName)";

                    var parameters = new {StackId = int.Parse(initialStackID), StackName = initialStackName};

                    connection.Execute(query, parameters);

                    StacksManager();
                }
        }

        public void DeleteStack(){
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Which row do you want to delete? Enter StackID number");
                    string choice = Console.ReadLine();

                    if(int.TryParse(choice, out int stackID)){
                        string query = "DELETE FROM dbo.STACKS WHERE StackID = @StackID";

                        int rowsAffected = connection.Execute(query, new { StackID = stackID });

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Stack with ID {stackID} deleted successfully.");
                            ResequenceStacks(connection);
                        }
                        else
                        {
                            Console.WriteLine($"No stack found with ID {stackID}.");
                        }
                    }

                    else{
                        Console.WriteLine("ERROR");
                    }

                }
        }

        private void ResequenceStacks(SqlConnection connection){
            var selectQuery = "SELECT * FROM dbo.STACKS ORDER BY StackID";
            var stacks = connection.Query(selectQuery).ToList();

            int newStackID = 1;

            foreach(var stack in stacks){
                string updateQuery = "UPDATE dbo.STACKS SET StackID = @NewStackID WHERE StackID = @OldStackId";
                connection.Execute(updateQuery, new { NewStackID = newStackID, OldStackID = stack.StackID });

                newStackID++;
            }
        }
    }
}
