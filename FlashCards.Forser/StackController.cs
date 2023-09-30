using FlashCards.Forser.DTOs;
using Microsoft.Data.SqlClient;

namespace FlashCards.Forser
{
    internal class StackController
    {
        internal void ShowStackMenu()
        {
            FlashcardController flashcardController = new FlashcardController();
            MainMenuController mainMenuController = new MainMenuController();

            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("              STACK MENU");
            Console.WriteLine("List - List all Stacks");
            if (CountAllStacks() > 10)
            {
                Console.WriteLine("Add - Add a new Stack");
                Console.WriteLine("Edit - Edit a Stack");
                Console.WriteLine("Delete - Delete a Stack\n");
            }
            Console.WriteLine("Flash - Go to Flashcard Menu");
            Console.WriteLine("Menu - Return to Main Menu");
            Console.WriteLine("------------------------------------------");

            string selectedStackMenu = Console.ReadLine().Trim().ToLower();

            switch (selectedStackMenu)
            {
                case "list":
                    ListAllStacks();
                    break;
                case "add":
                    AddNewStack();
                    break;
                case "edit":
                    break;
                case "delete":
                    break;
                case "menu":
                    mainMenuController.MainMenu();
                    break;
                case "flash":
                    flashcardController.ShowFlashcardMenu();
                    break;
            }
        }


        internal void AddNewStack()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Enter your new Stack name: ");
            string stackName = Console.ReadLine();

            StackDTO newStack = new StackDTO();
            newStack.Name = stackName;
            int rows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(new Startup().AppSettings.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"INSERT Stacks (Name) VALUES ('{newStack.Name}')";

                    connection.Open();
                    rows = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            if (rows > 0)
            {
                Console.WriteLine("Stack has been saved.");
                Console.WriteLine("Press ENTER to return to Stack Menu");
                Console.ReadLine();
                ShowStackMenu();

            }
        }

        internal int CountAllStacks()
        {
            int stackCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(new Startup().AppSettings.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "SELECT Count('Name') FROM Stacks";

                    connection.Open();
                    stackCount = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return stackCount;
        }

        internal void ListAllStacks()
        {
            List<StackDTO> stackList = new List<StackDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(new Startup().AppSettings.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "SELECT * FROM Stacks";

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        stackList.Add(new StackDTO
                        {
                            Name = reader[1].ToString()
                        });
                    }

                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            foreach (StackDTO stack in stackList)
            {
                Console.WriteLine($"Stack Name: {stack.Name}");
            }
            Console.ReadLine();
            ShowStackMenu();
        }
    }
}