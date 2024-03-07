using Flash.Helper.DTO;
using Flash.Helper.Renumber;
using System.Data.SqlClient;

namespace Flash.Helper.MainHelper;
internal class AllExistingStacks
{
    internal static void ShowAllExistingStacks()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            try
            {
                List<StacksDto> stacks = new List<StacksDto>();

                string showAllStacks = @"
                    SELECT Stack_Primary_Id, Name
                    FROM Stacks";

                using (SqlCommand command = new SqlCommand(showAllStacks, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                StacksDto stack = new StacksDto
                                {
                                    Stack_Primary_Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };
                                stacks.Add(stack);
                            }

                            Console.WriteLine("Stacks in the 'Stacks' Table:\n");

                            foreach (var stack in stacks)
                            {
                                Console.WriteLine($"Stack_Primary_Id: {stack.Stack_Primary_Id}, Name: {stack.Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No stacks found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}