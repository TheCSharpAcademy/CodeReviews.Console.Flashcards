namespace FlashCards.Ramseis
{
    internal class ManageAdd
    {
        public static void AddStack()
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Add Stack" },
                Options = new List<string> { "Enter new stack name below!", "Name must be unique and less than 256 characters.", "Enter Q to return to previous menu." }
            };
            menu.Draw();
            bool escape = true;
            while (escape)
            {
                string input = (Console.ReadLine() ?? string.Empty).Trim();
                if (input == "q" | input == "Q")
                {
                    escape = false;
                }
                else if (input.Length > 0 & input.Length < 256)
                {
                    List<CardStack> stacks = IO.SqlGetStacks();
                    bool exists = false;
                    foreach (CardStack stack in stacks)
                    {
                        if (stack.Name.Equals(input, StringComparison.CurrentCultureIgnoreCase))
                        {
                            exists = true;
                        }
                    }
                    if (exists)
                    {
                        menu.Draw("Input not accepted. Stack name already exists!");
                    }
                    else
                    {
                        IO.SqlAddStack(input);
                        escape = false;
                    }
                }
                else
                {
                    menu.Draw("Input not accepted. Please see directions above!");
                }
            }
        }
    }
}
