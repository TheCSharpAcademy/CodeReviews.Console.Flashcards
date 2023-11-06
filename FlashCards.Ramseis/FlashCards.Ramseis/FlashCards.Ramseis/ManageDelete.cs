namespace FlashCards.Ramseis
{
    internal class ManageDelete
    {
        public static void DeleteStack()
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Delete Stack", "", "Existing stacks" },
                Options = new List<string> { }
            };
            List<CardStack> stacks = IO.SqlGetStacks();
            foreach (CardStack stack in stacks)
            {
                menu.Options.Add("\"" + stack.Name + "\"");
            }
            menu.Options.Add("");
            menu.Options.Add("Enter stack name to delete. Enter Q to return to previous menu.");
            menu.Draw();
            bool escape = true;
            while (escape)
            {
                int deleteID = -1;
                string input = (Console.ReadLine() ?? string.Empty).Trim();
                if (input == "q" | input == "Q")
                {
                    escape = false;
                }
                else
                {
                    foreach (CardStack stack in stacks)
                    {
                        if (input == stack.Name)
                        {
                            deleteID = IO.SqlGetStackID(input);
                        }
                    }
                    if (deleteID > 0)
                    {
                        IO.SqlDeleteStack(deleteID);
                        escape = false;
                    }
                    else
                    {
                        menu.Draw("Input not accepted.");
                    }
                }
            }
        }
    }
}
