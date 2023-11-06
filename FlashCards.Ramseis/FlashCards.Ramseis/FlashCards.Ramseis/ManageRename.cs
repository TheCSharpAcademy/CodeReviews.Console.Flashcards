namespace FlashCards.Ramseis
{
    internal class ManageRename
    {
        public static void RenameMenu()
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Rename Stack", "", "Existing stacks" },
                Options = new List<string> { }
            };
            int stackCount = IO.SqLStackCount();
            if (stackCount < 1)
            {
                menu.Options.Add("No stacks are available!");
                menu.Options.Add("Return to the main menu, enter the manage option, and add a deck!");
                menu.Draw("Press any key to return to main menu...");
                Console.ReadKey();
                return;
            }

            Dictionary<int, int> stackIdMap = new();
            Dictionary<int, string> stackNameMap = new();
            List<CardStack> stacks = IO.SqlGetStacks();
            int key = 1;
            foreach (CardStack stack in stacks)
            {
                stackIdMap[key] = stack.ID;
                stackNameMap[key] = stack.Name;
                if (key < 10)
                {
                    menu.Options.Add(" " + key + ". \"" + stack.Name + "\"");
                }
                else
                {
                    menu.Options.Add(key + ". \"" + stack.Name + "\"");
                }
                key++;
            }
            int exitKey = key;
            if (exitKey < 10)
            {
                menu.Options.Add(" " + exitKey + ". Return to previous menu.");
            }
            else
            {
                menu.Options.Add(exitKey + ". Return to previous menu.");
            }
            menu.Options.Add("");
            menu.Options.Add("Select card stack to rename.");

            bool escape = true;
            int targetID = -1;
            int input1 = -1;
            menu.Draw();
            while (escape)
            {
                input1 = IO.GetInteger();
                if (input1 < exitKey)
                {
                    targetID = stackIdMap[input1];
                    escape = false;
                }
                else if (input1 == exitKey)
                {
                    return;
                }
                else
                {
                    menu.Draw("Input not recognized. Please select from the menu above!");
                }
            }

            menu.Titles = new List<string> { "-- Study Flashcards --", "Card Management", "Rename Stack" };
            menu.Options = new List<string> { "Enter new stack name" };
            menu.Draw($"Modifying stack \"{stackNameMap[input1]}\"");

            escape = true;
            while (escape)
            {
                string input = (Console.ReadLine() ?? string.Empty).Trim();
                bool exists = false;
                foreach (CardStack stack in stacks)
                {
                    if (input.Equals(stack.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        exists = true;
                    }
                }
                if (input.Length > 0 & !exists)
                {
                    IO.SqlRenameStack(targetID, input);
                    escape = false;
                }
                else if (exists)
                {
                    menu.Draw("Name already exists!");
                }
                else
                {
                    menu.Draw("Input not accepted.");
                }
            }
        }
    }
}
