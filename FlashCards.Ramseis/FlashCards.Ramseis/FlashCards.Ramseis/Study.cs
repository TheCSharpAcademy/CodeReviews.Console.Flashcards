namespace FlashCards.Ramseis
{
    internal class Study
    {
        public static void Menu()
        {
            Menu menu = new Menu
            {
                Titles = new List<string> { "-- Study Flashcards --", "Study Room" },
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
            menu.Options.Add("Select card stack to study.");

            bool escape = true;
            menu.Draw();
            while (escape)
            {
                int input = IO.GetInteger();
                if (input < exitKey)
                {

                    StudyStack.Play(stackIdMap[input], stackNameMap[input]);
                    escape = false;
                }
                else if (input == exitKey)
                {
                    escape = false;
                }
                else
                {
                    menu.Draw("Input not recognized. Please select from the menu above!");
                }
            }
        }
    }
}